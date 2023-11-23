package hu.bme.aut.android.homeworkmanagerapp.feature.auth.login

import androidx.lifecycle.ViewModel
import dagger.hilt.android.lifecycle.HiltViewModel
import hu.bme.aut.android.homeworkmanagerapp.feature.auth.AuthHandler
import javax.inject.Inject

@HiltViewModel
class LoginViewModel @Inject constructor(
    private val authHandler: AuthHandler
) : ViewModel() {
    fun login(username: String, password: String, onLogin: () -> Unit, onError: () -> Unit) {
        authHandler.login(username, password, onLogin, onError)
    }
}