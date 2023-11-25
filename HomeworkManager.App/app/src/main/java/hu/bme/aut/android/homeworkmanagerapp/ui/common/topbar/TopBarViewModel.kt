package hu.bme.aut.android.homeworkmanagerapp.ui.common.topbar

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import dagger.hilt.android.lifecycle.HiltViewModel
import hu.bme.aut.android.homeworkmanagerapp.feature.auth.AuthHandler
import kotlinx.coroutines.launch
import javax.inject.Inject

@HiltViewModel
class TopBarViewModel @Inject constructor(
    private val authHandler: AuthHandler
) : ViewModel() {

    fun logout(onLogout: () -> Unit) {
        viewModelScope.launch {
            authHandler.logout(onLogout)
        }
    }
}