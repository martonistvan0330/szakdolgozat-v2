package hu.bme.aut.android.homeworkmanagerapp.feature.appointment.grid

import android.content.Context
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.microsoft.signalr.HubConnection
import com.microsoft.signalr.HubConnectionState
import dagger.hilt.android.lifecycle.HiltViewModel
import dagger.hilt.android.qualifiers.ApplicationContext
import hu.bme.aut.android.homeworkmanagerapp.R
import hu.bme.aut.android.homeworkmanagerapp.feature.appointment.AppointmentHandler
import hu.bme.aut.android.homeworkmanagerapp.ui.model.appointment.AppointmentRowUi
import hu.bme.aut.android.homeworkmanagerapp.ui.model.appointment.asAppointmentRowUi
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import javax.inject.Inject

sealed class AppointmentGridState {
    object Init : AppointmentGridState()
    object Loading : AppointmentGridState()
    data class Error(val error: Throwable) : AppointmentGridState()
    data class Result(val appointmentRows: List<AppointmentRowUi>) : AppointmentGridState()
}

@HiltViewModel
class AppointmentGridViewModel @Inject constructor(
    private val appointmentHandler: AppointmentHandler,
    private val hubConnection: HubConnection,
    @ApplicationContext private val context: Context
) : ViewModel() {
    private val _state = MutableStateFlow<AppointmentGridState>(AppointmentGridState.Init)
    val state = _state.asStateFlow()

    init {
        viewModelScope.launch {
            hubConnection.start().blockingAwait()
        }
    }

    fun initialize(assignmentId: Int) {
        _state.value = AppointmentGridState.Loading

        hubConnection.on("Refresh") {
            loadAppointments(assignmentId)
        }

        if (hubConnection.connectionState == HubConnectionState.CONNECTED) {
            hubConnection.invoke("JoinRoom", assignmentId)
        }
    }

    fun signUp(appointmentId: Int) {
        viewModelScope.launch {
            appointmentHandler.signUp(
                appointmentId,
                onSuccess = { _ ->
                },
                onError = {
                    _state.value =
                        AppointmentGridState.Error(
                            error = Exception(context.getString(R.string.something_went_wrong))
                        )
                }
            )
        }
    }

    fun stopConnection() {
        hubConnection.stop()
    }

    private fun loadAppointments(assignmentId: Int) {
        viewModelScope.launch {
            appointmentHandler.getAppointments(
                assignmentId,
                onSuccess = { result ->
                    _state.value = AppointmentGridState.Result(
                        appointmentRows = result.map {
                            it.asAppointmentRowUi()
                        }
                    )
                },
                onError = {
                    _state.value =
                        AppointmentGridState.Error(
                            error = Exception(context.getString(R.string.something_went_wrong))
                        )
                }
            )
        }
    }
}