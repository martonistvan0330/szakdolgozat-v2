package hu.bme.aut.android.homeworkmanagerapp.domain.model.appointment

data class AppointmentModel(
    val appointmentId: Int,
    val time: String,
    val teacherName: String,
    val teacherEmail: String,
    val isAvailable: Boolean,
    val isMine: Boolean
)