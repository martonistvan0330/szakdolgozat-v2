package hu.bme.aut.android.homeworkmanagerapp.ui.model.appointment

import hu.bme.aut.android.homeworkmanagerapp.domain.model.appointment.AppointmentModel

data class AppointmentModelUi(
    val appointmentId: Int,
    val time: String,
    val teacherName: String,
    val teacherEmail: String,
    val isAvailable: Boolean,
    val isMine: Boolean
)

fun AppointmentModel.asAppointmentModelUi(): AppointmentModelUi {
    return AppointmentModelUi(
        appointmentId = appointmentId,
        time = time,
        teacherName = teacherName,
        teacherEmail = teacherEmail,
        isAvailable = isAvailable,
        isMine = isMine
    )
}