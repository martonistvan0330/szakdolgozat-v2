package hu.bme.aut.android.homeworkmanagerapp.network.appointment

import hu.bme.aut.android.homeworkmanagerapp.domain.model.appointment.AppointmentRow
import retrofit2.http.GET
import retrofit2.http.PATCH
import retrofit2.http.Path

interface AppointmentApi {
    @GET("/api/Appointment/{assignmentId}")
    suspend fun getAppointments(
        @Path("assignmentId") assignmentId: Int
    ): List<AppointmentRow>

    @PATCH("/api/Appointment/{appointmentId}")
    suspend fun signUp(
        @Path("appointmentId") appointmentId: Int
    ): Int
}