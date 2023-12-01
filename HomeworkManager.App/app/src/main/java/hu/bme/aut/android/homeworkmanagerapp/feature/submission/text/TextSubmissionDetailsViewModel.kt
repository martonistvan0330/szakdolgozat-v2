package hu.bme.aut.android.homeworkmanagerapp.feature.submission.text

import android.content.Context
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import dagger.hilt.android.lifecycle.HiltViewModel
import dagger.hilt.android.qualifiers.ApplicationContext
import hu.bme.aut.android.homeworkmanagerapp.R
import hu.bme.aut.android.homeworkmanagerapp.domain.model.submission.UpdatedTextSubmission
import hu.bme.aut.android.homeworkmanagerapp.feature.assignment.details.AssignmentDetailsState
import hu.bme.aut.android.homeworkmanagerapp.feature.submission.SubmissionHandler
import hu.bme.aut.android.homeworkmanagerapp.ui.model.submission.TextSubmissionModelUi
import hu.bme.aut.android.homeworkmanagerapp.ui.model.submission.asTextSubmissionModelUi
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import javax.inject.Inject

sealed class TextSubmissionDetailsState {
    object Loading : TextSubmissionDetailsState()
    object Unauthorized : TextSubmissionDetailsState()
    data class Error(val error: Throwable) : TextSubmissionDetailsState()
    data class Result(val submission: TextSubmissionModelUi) : TextSubmissionDetailsState()
}

@HiltViewModel
class TextSubmissionDetailsViewModel @Inject constructor(
    private val submissionHandler: SubmissionHandler,
    @ApplicationContext private val context: Context
) : ViewModel() {
    private val _state =
        MutableStateFlow<TextSubmissionDetailsState>(TextSubmissionDetailsState.Loading)
    val state = _state.asStateFlow()


    private val _loadingState = MutableStateFlow(false)
    val loadingState = _loadingState.asStateFlow()

    private val _answerState = MutableStateFlow("")
    val answerState = _answerState.asStateFlow()

    fun updateAnswer(newValue: String) {
        _answerState.value = newValue
    }

    fun loadSubmission(assignmentId: Int) {
        viewModelScope.launch {
            submissionHandler.getTextAssignment(
                assignmentId = assignmentId,
                onSuccess = { result ->
                    _state.value = TextSubmissionDetailsState.Result(
                        submission = result.asTextSubmissionModelUi()
                    )
                    _answerState.value = result.answer
                },
                onError = {
                    AssignmentDetailsState.Error(
                        Exception(context.getString(R.string.something_went_wrong))
                    )
                }
            )
        }
    }

    fun save(
        assignmentId: Int,
        onSuccess: (Int) -> Unit = {},
        onError: () -> Unit = {}
    ) {
        viewModelScope.launch {
            _loadingState.value = true
            submissionHandler.upsertTextAssignment(
                updatedTextSubmission = UpdatedTextSubmission(
                    assignmentId = assignmentId,
                    answer = answerState.value
                ),
                onSuccess = { submissionId ->
                    loadSubmission(assignmentId)
                    _loadingState.value = false
                    onSuccess(submissionId)
                },
                onError = {
                    _loadingState.value = false
                    onError()
                }
            )
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