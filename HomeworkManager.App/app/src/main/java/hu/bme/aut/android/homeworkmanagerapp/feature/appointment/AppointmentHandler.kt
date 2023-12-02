package hu.bme.aut.android.homeworkmanagerapp.feature.appointment

import hu.bme.aut.android.homeworkmanagerapp.domain.model.appointment.AppointmentRow
import hu.bme.aut.android.homeworkmanagerapp.network.RefreshService
import hu.bme.aut.android.homeworkmanagerapp.network.appointment.AppointmentNetworkManager
import hu.bme.aut.android.homeworkmanagerapp.network.handleAuthorizedRequest
import retrofit2.HttpException
import javax.inject.Inject

class AppointmentHandler @Inject constructor(
    private val appointmentNetworkManager: AppointmentNetworkManager,
    private val refreshService: RefreshService
) {
    suspend fun getAppointments(
        assignmentId: Int,
        onSuccess: (List<AppointmentRow>) -> Unit,
        onError: () -> Unit
    ) {
        try {
            val appointments = handleAuthorizedRequest(refreshService) {
                appointmentNetworkManager.getAppointments(assignmentId)
            }

            onSuccess(appointments)
        } catch (httpException: HttpException) {
            onError()
        }
    }

    suspend fun signUp(
        appointmentId: Int,
        onSuccess: (Int) -> Unit,
        onError: () -> Unit
    ) {
        try {
            val chosenAppointmentId = handleAuthorizedRequest(refreshService) {
                appointmentNetworkManager.signUp(appointmentId)
            }

            onSuccess(chosenAppointmentId)
        } catch (httpException: HttpException) {
            onError()
        }
    }
}