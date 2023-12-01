package hu.bme.aut.android.homeworkmanagerapp.feature.submission.file

import android.content.Context
import android.net.Uri
import android.os.Build
import android.provider.OpenableColumns
import androidx.annotation.RequiresApi
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import dagger.hilt.android.lifecycle.HiltViewModel
import dagger.hilt.android.qualifiers.ApplicationContext
import hu.bme.aut.android.homeworkmanagerapp.R
import hu.bme.aut.android.homeworkmanagerapp.feature.submission.SubmissionHandler
import hu.bme.aut.android.homeworkmanagerapp.ui.model.submission.FileSubmissionModelUi
import hu.bme.aut.android.homeworkmanagerapp.ui.model.submission.asFileSubmissionModelUi
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import javax.inject.Inject

sealed class FileSubmissionDetailsState {
    object Loading : FileSubmissionDetailsState()
    data class Error(val error: Throwable) : FileSubmissionDetailsState()
    data class Result(val submission: FileSubmissionModelUi) : FileSubmissionDetailsState()
}

@HiltViewModel
class FileSubmissionDetailsViewModel @Inject constructor(
    private val submissionHandler: SubmissionHandler,
    @ApplicationContext private val context: Context
) : ViewModel() {
    private val _state =
        MutableStateFlow<FileSubmissionDetailsState>(FileSubmissionDetailsState.Loading)
    val state = _state.asStateFlow()

    private val _loadingState = MutableStateFlow(false)
    val loadingState = _loadingState.asStateFlow()

    fun loadSubmission(assignmentId: Int) {
        viewModelScope.launch {
            submissionHandler.getFileAssignment(
                assignmentId = assignmentId,
                onSuccess = { result ->
                    _state.value = FileSubmissionDetailsState.Result(
                        submission = result.asFileSubmissionModelUi()
                    )
                },
                onError = {
                    FileSubmissionDetailsState.Error(
                        Exception(context.getString(R.string.something_went_wrong))
                    )
                }
            )
        }
    }

    fun download(assignmentId: Int, uri: Uri?) {
        if (uri != null) {
            viewModelScope.launch {
                submissionHandler.downloadFile(
                    assignmentId = assignmentId,
                    onSuccess = { bytes ->
                        context.contentResolver.openOutputStream(uri)?.use {
                            it.write(bytes)
                        }
                    },
                    onError = {
                        FileSubmissionDetailsState.Error(
                            Exception(context.getString(R.string.something_went_wrong))
                        )
                    }
                )
            }
        }
    }

    @RequiresApi(Build.VERSION_CODES.Q)
    fun upload(assignmentId: Int, uri: Uri?) {
        if (uri != null) {
            val path = uri.path
            if (path != null) {
                viewModelScope.launch {
                    context.contentResolver.openInputStream(uri)?.use { inputStream ->
                        val bytes = inputStream.readBytes()

                        context.contentResolver.query(
                            uri,
                            null,
                            null,
                            null,
                            null
                        )?.use { cursor ->
                            val nameIndex = cursor.getColumnIndex(OpenableColumns.DISPLAY_NAME)
                            cursor.moveToFirst()
                            val name = cursor.getString(nameIndex)

                            submissionHandler.uploadFile(
                                assignmentId = assignmentId,
                                name,
                                bytes,
                                onSuccess = { _ ->
                                    loadSubmission(assignmentId)
                                },
                                onError = {
                                    FileSubmissionDetailsState.Error(
                                        Exception(context.getString(R.string.something_went_wrong))
                                    )
                                }
                            )
                        }
                    }
                }
            }
        }
    }

    fun submit(
        assignmentId: Int,
        onSuccess: () -> Unit = {},
        onError: () -> Unit = {}
    ) {
        viewModelScope.launch {
            _loadingState.value = true
            submissionHandler.submit(
                assignmentId = assignmentId,
                onSuccess = {
                    loadSubmission(assignmentId)
                    _loadingState.value = false
                    onSuccess()
                },
                onError = {
                    _loadingState.value = false
                    onError()
                }
            )
        }
    }
}