package hu.bme.aut.android.homeworkmanagerapp.feature.appointment.grid

import androidx.lifecycle.ViewModel
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import javax.inject.Inject

sealed class AppointmentGridState {
    object Loading : AppointmentGridState()
    data class Error(val error: Throwable) : AppointmentGridState()
    data class Result(val appointments: String) : AppointmentGridState()
}

@HiltViewModel
class AppointmentGridViewModel @Inject constructor(

) : ViewModel() {
    private val _state = MutableStateFlow<AppointmentGridState>(AppointmentGridState.Loading)
    val state = _state.asStateFlow()

    fun loadAppointments(assignmentId: Int) {

    }
}