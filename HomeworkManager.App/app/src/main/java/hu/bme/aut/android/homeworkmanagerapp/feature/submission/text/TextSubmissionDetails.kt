package hu.bme.aut.android.homeworkmanagerapp.feature.submission.text

import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.Button
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.material3.TextField
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import androidx.hilt.navigation.compose.hiltViewModel
import hu.bme.aut.android.homeworkmanagerapp.R

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun TextSubmissionDetails(
    assignmentId: Int,
    modifier: Modifier = Modifier,
    viewModel: TextSubmissionDetailsViewModel = hiltViewModel()
) {
    Box(
        modifier = modifier.fillMaxSize()
    ) {
        when (val state = viewModel.state.collectAsState().value) {
            is TextSubmissionDetailsState.Loading -> {
                CircularProgressIndicator(
                    modifier = Modifier.align(Alignment.Center),
                    color = MaterialTheme.colorScheme.onSecondaryContainer
                )
                viewModel.loadSubmission(assignmentId)
            }

            is TextSubmissionDetailsState.Unauthorized -> Text(
                text = stringResource(id = R.string.unauthorized)
            )

            is TextSubmissionDetailsState.Error -> Text(
                text = state.error.toString()
            )

            is TextSubmissionDetailsState.Result -> {
                val answer by viewModel.answerState.collectAsState()
                val submission = state.submission

                Column(
                    modifier = Modifier.fillMaxWidth()
                ) {
                    val loading by viewModel.loadingState.collectAsState()

                    val modifier = Modifier
                        .fillMaxWidth()
                        .padding(vertical = 8.dp)

                    TextField(
                        modifier = modifier,
                        value = answer,
                        onValueChange = { viewModel.updateAnswer(it) },
                        enabled = submission.isDraft && !loading,
                        singleLine = false,
                    )
                    Button(
                        modifier = modifier,
                        onClick = {
                            viewModel.save(assignmentId)
                        },
                        enabled = submission.isDraft && !loading
                    ) {
                        Text(text = stringResource(R.string.button_label_save))
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