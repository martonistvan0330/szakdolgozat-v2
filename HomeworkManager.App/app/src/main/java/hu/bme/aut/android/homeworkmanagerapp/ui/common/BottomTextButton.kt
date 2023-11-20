package hu.bme.aut.android.homeworkmanagerapp.ui.common

import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.semantics.Role
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp

@Composable
fun BottomTextButton(
    modifier: Modifier = Modifier,
    onClick: () -> Unit,
    enabled: Boolean = true,
    label: String,
) {
    val shape = RoundedCornerShape(topEnd = 5.dp, topStart = 5.dp)
    Surface(
        shape = shape,
        modifier = modifier
            .fillMaxWidth()
            .clip(shape)
            .clickable(
                enabled = enabled,
                onClick = onClick,
                role = Role.Button,
            ),
        color = MaterialTheme.colorScheme.primaryContainer,
    ) {
        Text(
            text = label,
            modifier = Modifier.padding(10.dp),
            textAlign = TextAlign.Center,
            color = MaterialTheme.colorScheme.onPrimaryContainer,
            style = MaterialTheme.typography.labelLarge,
            fontSize = 16.sp,
        )
    }
}

@Preview(showBackground = true)
@Composable
fun TextButton_Preview() {
    Box(modifier = Modifier.fillMaxSize()) {
        BottomTextButton(
            onClick = {},
            modifier = Modifier.align(Alignment.BottomCenter),
            label = "Button",
        )
    }
}