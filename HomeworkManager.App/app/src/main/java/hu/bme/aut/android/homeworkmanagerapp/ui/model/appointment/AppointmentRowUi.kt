package hu.bme.aut.android.homeworkmanagerapp.ui.model.appointment

import hu.bme.aut.android.homeworkmanagerapp.domain.model.appointment.AppointmentRow

data class AppointmentRowUi(
    val date: String,
    val appointmentModels: List<AppointmentModelUi>
)

fun AppointmentRow.asAppointmentRowUi(): AppointmentRowUi {
    return AppointmentRowUi(
        date = date,
        appointmentModels = appointmentModels.map { it.asAppointmentModelUi() }
    )
}