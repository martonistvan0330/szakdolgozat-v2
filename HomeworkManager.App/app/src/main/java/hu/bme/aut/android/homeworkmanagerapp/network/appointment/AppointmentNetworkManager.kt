package hu.bme.aut.android.homeworkmanagerapp.network.appointment

import hu.bme.aut.android.homeworkmanagerapp.domain.model.appointment.AppointmentRow
import javax.inject.Inject

class AppointmentNetworkManager @Inject constructor(
    private val appointmentApi: AppointmentApi
) {
    suspend fun getAppointments(assignmentId: Int): List<AppointmentRow> {
        return appointmentApi.getAppointments(assignmentId)
            .map {
                it.copy(
                    date = it.date.split('T')[0]
                )
            }
    }

    suspend fun signUp(appointmentId: Int): Int {
        return appointmentApi.signUp(appointmentId)
    }
}