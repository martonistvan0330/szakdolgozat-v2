package hu.bme.aut.android.homeworkmanagerapp.feature.appointment.grid

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.stringResource
import androidx.hilt.navigation.compose.hiltViewModel
import androidx.navigation.NavHostController
import hu.bme.aut.android.homeworkmanagerapp.R
import hu.bme.aut.android.homeworkmanagerapp.ui.common.bottombar.BottomBar
import hu.bme.aut.android.homeworkmanagerapp.ui.common.topbar.TopBar

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
                title = stringResource(id = R.string.text_your_groups_list),
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
                is AppointmentGridState.Loading -> {
                    CircularProgressIndicator(
                        modifier = Modifier.align(Alignment.Center),
                        color = MaterialTheme.colorScheme.onSecondaryContainer
                    )
                    viewModel.loadAppointments(assignmentId)
                }

                is AppointmentGridState.Error -> Text(
                    text = state.error.toString()
                )

                is AppointmentGridState.Result -> {
                    Text("works")
                }
            }
        }
    }
}