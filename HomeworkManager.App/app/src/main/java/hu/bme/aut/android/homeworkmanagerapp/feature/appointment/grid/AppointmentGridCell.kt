package hu.bme.aut.android.homeworkmanagerapp.feature.appointment.grid

import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.fillMaxHeight
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.width
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.RectangleShape
import androidx.compose.ui.semantics.Role
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import hu.bme.aut.android.homeworkmanagerapp.ui.model.appointment.AppointmentModelUi

@Composable
fun AppointmentGridCell(
    appointment: AppointmentModelUi,
    onClick: (AppointmentModelUi) -> Unit
) {
    Surface(
        shape = RectangleShape,
        modifier = Modifier
            .fillMaxHeight()
            .width(100.dp)
            .padding(all = 8.dp)
            .background(color = MaterialTheme.colorScheme.background)
            .clickable(
                enabled = appointment.isAvailable,
                onClick = { onClick(appointment) },
                role = Role.Button,
            ),
        color = if (appointment.isMine)
            MaterialTheme.colorScheme.tertiary
        else if (appointment.isAvailable)
            MaterialTheme.colorScheme.surfaceTint
        else
            MaterialTheme.colorScheme.surfaceVariant
    )
    {
        Box {
            Text(
                text = appointment.time,
                textAlign = TextAlign.Center,
                color = if (appointment.isMine)
                    MaterialTheme.colorScheme.onTertiary
                else if (appointment.isAvailable)
                    MaterialTheme.colorScheme.onSurface
                else
                    MaterialTheme.colorScheme.onSurfaceVariant,
                style = MaterialTheme.typography.labelLarge,
                fontSize = 24.sp,
                modifier = Modifier.align(Alignment.Center)
            )
        }
    }
}