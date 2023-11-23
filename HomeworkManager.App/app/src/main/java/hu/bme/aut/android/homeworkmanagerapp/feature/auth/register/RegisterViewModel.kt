package hu.bme.aut.android.homeworkmanagerapp.feature.auth.register

import androidx.lifecycle.ViewModel
import dagger.hilt.android.lifecycle.HiltViewModel
import hu.bme.aut.android.homeworkmanagerapp.feature.auth.AuthHandler
import javax.inject.Inject

@HiltViewModel
class RegisterViewModel @Inject constructor(
    private val authHandler: AuthHandler
) : ViewModel() {
    fun register(
        firstName: String,
        lastName: String,
        username: String,
        password: String,
        email: String,
        onSuccess: () -> Unit,
        onError: () -> Unit
    ) {
        authHandler.register(firstName, lastName, email, username, password, onSuccess, onError)
    }
}