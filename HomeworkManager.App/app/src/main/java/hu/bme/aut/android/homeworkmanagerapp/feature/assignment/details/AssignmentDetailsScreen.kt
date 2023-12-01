package hu.bme.aut.android.homeworkmanagerapp.feature.assignment.details

import android.os.Build
import androidx.annotation.RequiresApi
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.Button
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.Divider
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import androidx.hilt.navigation.compose.hiltViewModel
import androidx.navigation.NavHostController
import hu.bme.aut.android.homeworkmanagerapp.R
import hu.bme.aut.android.homeworkmanagerapp.feature.submission.SubmissionDetails
import hu.bme.aut.android.homeworkmanagerapp.navigation.Screen
import hu.bme.aut.android.homeworkmanagerapp.ui.common.bottombar.BottomBar
import hu.bme.aut.android.homeworkmanagerapp.ui.common.topbar.TopBar

@RequiresApi(Build.VERSION_CODES.Q)
@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun AssignmentDetailsScreen(
    assignmentId: Int,
    onLogout: () -> Unit,
    navController: NavHostController,
    viewModel: AssignmentDetailsViewModel = hiltViewModel()
) {
    val state = viewModel.state.collectAsState().value
    viewModel.loadAssignment(assignmentId)

    Scaffold(
        topBar = {
            TopBar(
                title = if (state is AssignmentDetailsState.Result) state.assignment.name else "",
                onLogout = onLogout
            )
        },
        bottomBar = {
            BottomBar(
                navController = navController
            )
        },
        snackbarHost = {},
        modifier = Modifier.fillMaxSize()
    ) { padding ->
        Box(
            modifier = Modifier
                .fillMaxSize()
                .padding(padding)
                .background(
                    color = if (state is AssignmentDetailsState.Error) {
                        MaterialTheme.colorScheme.secondaryContainer
                    } else {
                        MaterialTheme.colorScheme.background
                    }
                ),
            contentAlignment = Alignment.TopCenter,
        ) {
            when (state) {
                is AssignmentDetailsState.Loading -> CircularProgressIndicator(
                    modifier = Modifier.align(Alignment.Center),
                    color = MaterialTheme.colorScheme.onSecondaryContainer
                )

                is AssignmentDetailsState.Error -> Text(
                    text = state.error.toString()
                )

                is AssignmentDetailsState.Result -> {
                    val assignment = state.assignment
                    Column(
                        modifier = Modifier
                            .fillMaxWidth()
                            .padding(all = 16.dp)
                    ) {
                        val modifier = Modifier.padding(vertical = 8.dp)
                        Row(
                            modifier = modifier,
                            horizontalArrangement = Arrangement.SpaceBetween,
                            verticalAlignment = Alignment.CenterVertically
                        ) {
                            Text(text = "Course:")
                            Text(text = assignment.courseName)
                        }
                        Row(
                            modifier = modifier,
                            horizontalArrangement = Arrangement.SpaceBetween,
                            verticalAlignment = Alignment.CenterVertically
                        ) {
                            Text(text = "Group:")
                            Text(text = assignment.groupName)
                        }
                        Row(
                            modifier = modifier,
                            horizontalArrangement = Arrangement.SpaceBetween,
                            verticalAlignment = Alignment.CenterVertically
                        ) {
                            Text(text = "Deadline:")
                            Text(text = assignment.deadline)
                        }
                        if (assignment.presentationRequired) {
                            Button(
                                modifier = modifier,
                                onClick = {
                                    navController.navigate("${Screen.AppointmentGrid.navigationRoute}/${assignmentId}")
                                }
                            ) {
                                Text(text = stringResource(R.string.assignment_details_presentation_appointments))
                            }
                        }
                        Divider(
                            modifier = modifier,
                            thickness = 2.dp
                        )
                        SubmissionDetails(
                            assignmentId = assignmentId,
                            assignmentTypeId = assignment.assignmentTypeId,
                            modifier = modifier
                        )
                    }
                }
            }
        }
    }
}