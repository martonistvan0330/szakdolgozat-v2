package hu.bme.aut.android.homeworkmanagerapp.feature.submission.file

import android.os.Build
import androidx.activity.compose.rememberLauncherForActivityResult
import androidx.activity.result.contract.ActivityResultContracts
import androidx.annotation.RequiresApi
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.Button
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import androidx.hilt.navigation.compose.hiltViewModel
import hu.bme.aut.android.homeworkmanagerapp.R

@RequiresApi(Build.VERSION_CODES.Q)
@Composable
fun FileSubmissionDetails(
    assignmentId: Int,
    modifier: Modifier = Modifier,
    viewModel: FileSubmissionDetailsViewModel = hiltViewModel()
) {
    Box(
        modifier = modifier.fillMaxSize()
    ) {
        when (val state = viewModel.state.collectAsState().value) {
            is FileSubmissionDetailsState.Loading -> {
                CircularProgressIndicator(
                    modifier = Modifier.align(Alignment.Center),
                    color = MaterialTheme.colorScheme.onSecondaryContainer
                )
                viewModel.loadSubmission(assignmentId)
            }

            is FileSubmissionDetailsState.Error -> Text(
                text = state.error.toString()
            )

            is FileSubmissionDetailsState.Result -> {
                val submission = state.submission

                Column(
                    modifier = Modifier.fillMaxWidth()
                ) {
                    val loading by viewModel.loadingState.collectAsState()

                    val modifier = Modifier
                        .fillMaxWidth()
                        .padding(vertical = 8.dp)

                    if (!submission.fileName.isNullOrEmpty()) {
                        val saveFile = rememberLauncherForActivityResult(
                            contract = ActivityResultContracts.CreateDocument("application/octet-stream")
                        ) { uri ->
                            viewModel.download(assignmentId, uri)
                        }

                        Button(
                            modifier = modifier,
                            onClick = {
                                saveFile.launch(submission.fileName)
                            },
                            enabled = !loading
                        ) {
                            Text(text = stringResource(R.string.button_download_submission))
                        }
                    }

                    val uploadFile = rememberLauncherForActivityResult(
                        contract = ActivityResultContracts.OpenDocument()
                    ) { uri ->
                        viewModel.upload(assignmentId, uri)
                    }

                    Button(
                        modifier = modifier,
                        onClick = {
                            uploadFile.launch(arrayOf("application/*", "image/*"))
                        },
                        enabled = submission.isDraft && !loading
                    ) {
                        Text(text = stringResource(R.string.button_upload))
                    }
                    Button(
                        modifier = modifier,
                        onClick = {
                            viewModel.submit(assignmentId)
                        },
                        enabled = submission.isDraft && !loading
                    ) {
                        Text(text = stringResource(R.string.button_label_submit))
                    }
                }
            }
        }
    }
}