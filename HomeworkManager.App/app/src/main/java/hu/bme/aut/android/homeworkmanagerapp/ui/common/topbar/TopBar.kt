package hu.bme.aut.android.homeworkmanagerapp.ui.common.topbar

import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Logout
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.Text
import androidx.compose.material3.TopAppBar
import androidx.compose.runtime.Composable
import androidx.hilt.navigation.compose.hiltViewModel

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun TopBar(
    title: String,
    onLogout: () -> Unit,
    viewModel: TopBarViewModel = hiltViewModel()
) {
    TopAppBar(
        title = {
            Text(text = title)
        },
        actions = {
            IconButton(onClick = {
                viewModel.logout(onLogout)
            }) {
                Icon(Icons.Filled.Logout, "logout")
            }
        }
    )
}