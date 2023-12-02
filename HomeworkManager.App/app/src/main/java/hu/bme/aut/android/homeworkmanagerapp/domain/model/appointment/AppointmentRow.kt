package hu.bme.aut.android.homeworkmanagerapp.domain.model.appointment

data class AppointmentRow(
    val date: String,
    val appointmentModels: List<AppointmentModel>
)