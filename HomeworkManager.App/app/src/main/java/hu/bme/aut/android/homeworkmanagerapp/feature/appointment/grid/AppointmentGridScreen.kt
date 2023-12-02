package hu.bme.aut.android.homeworkmanagerapp.feature.appointment.grid

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.material3.AlertDialog
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.material3.TextButton
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.stringResource
import androidx.hilt.navigation.compose.hiltViewModel
import androidx.navigation.NavHostController
import hu.bme.aut.android.homeworkmanagerapp.R
import hu.bme.aut.android.homeworkmanagerapp.ui.common.bottombar.BottomBar
import hu.bme.aut.android.homeworkmanagerapp.ui.common.topbar.TopBar
import hu.bme.aut.android.homeworkmanagerapp.ui.model.appointment.AppointmentModelUi

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun AppointmentGridScreen(
    assignmentId: Int,
    onLogout: () -> Unit,
    navController: NavHostController,
    viewModel: AppointmentGridViewModel = hiltViewModel()
) {
    val state = viewModel.state.collectAsState().value

    Scaffold(
        topBar = {
            TopBar(
                title = stringResource(id = R.string.text_appointments),
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
                    color = if (state is AppointmentGridState.Error) {
                        MaterialTheme.colorScheme.secondaryContainer
                    } else {
                        MaterialTheme.colorScheme.background
                    }
                ),
            contentAlignment = Alignment.TopCenter,
        ) {
            when (state) {
                is AppointmentGridState.Init -> viewModel.initialize(assignmentId)
                is AppointmentGridState.Loading -> {
                    CircularProgressIndicator(
                        modifier = Modifier.align(Alignment.Center),
                        color = MaterialTheme.colorScheme.onSecondaryContainer
                    )
                }

                is AppointmentGridState.Error -> Text(
                    text = state.error.toString()
                )

                is AppointmentGridState.Result -> {
                    var chosenAppointment by remember { mutableStateOf<AppointmentModelUi?>(null) }

                    if (chosenAppointment != null) {
                        AlertDialog(
                            title = {
                                Text(text = chosenAppointment!!.time)
                            },
                            text = {
                                Column {
                                    Text(text = chosenAppointment!!.teacherName)
                                    Text(text = chosenAppointment!!.teacherEmail)
                                }
                            },
                            onDismissRequest = {
                                chosenAppointment = null
                            },
                            dismissButton = {
                                TextButton(onClick = { chosenAppointment = null }) {
                                    Text(text = stringResource(R.string.cancel))
                                }
                            },
                            confirmButton = {
                                TextButton(
                                    onClick = {
                                        viewModel.signUp(chosenAppointment!!.appointmentId)
                                        chosenAppointment = null
                                    }
                                ) {
                                    Text(text = stringResource(R.string.choose))
                                }
                            }
                        )
                    }

                    LazyColumn(
                        modifier = Modifier.fillMaxSize()
                    ) {
                        items(
                            state.appointmentRows,
                            key = { appointmentRow -> appointmentRow.date }) { appointmentRow ->
                            AppointmentGridRow(
                                appointmentRow = appointmentRow,
                                onClick = { appointment ->
                                    chosenAppointment = appointment
                                }
                            )
                        }
                    }
                }
            }
        }
    }
}