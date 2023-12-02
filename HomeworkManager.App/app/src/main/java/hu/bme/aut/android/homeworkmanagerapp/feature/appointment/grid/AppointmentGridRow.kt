package hu.bme.aut.android.homeworkmanagerapp.feature.appointment.grid

import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.lazy.LazyRow
import androidx.compose.foundation.lazy.items
import androidx.compose.material3.Divider
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import hu.bme.aut.android.homeworkmanagerapp.ui.model.appointment.AppointmentModelUi
import hu.bme.aut.android.homeworkmanagerapp.ui.model.appointment.AppointmentRowUi

@Composable
fun AppointmentGridRow(
    appointmentRow: AppointmentRowUi,
    onClick: (AppointmentModelUi) -> Unit
) {
    Text(
        text = appointmentRow.date,
        style = MaterialTheme.typography.labelLarge,
        fontSize = 24.sp,
        modifier = Modifier.padding(start = 16.dp)
    )
    LazyRow(
        modifier = Modifier
            .height(100.dp)
            .padding(start = 16.dp)
    ) {
        items(
            appointmentRow.appointmentModels,
            key = { appointment -> appointment.appointmentId }
        ) { appointment ->
            AppointmentGridCell(
                appointment = appointment,
                onClick = onClick
            )
        }
    }
    Divider(
        thickness = 2.dp
    )
}