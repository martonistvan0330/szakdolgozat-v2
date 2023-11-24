package hu.bme.aut.android.homeworkmanagerapp.ui.model.auth

import hu.bme.aut.android.homeworkmanagerapp.domain.model.auth.AuthenticationRequest

data class LoginUiState(
    val username: String = "",
    val isUsernameError: Boolean = false,
    val password: String = "",
    val isPasswordError: Boolean = false,
    val isPasswordVisible: Boolean = false
)

fun LoginUiState.asAuthenticationRequest(): AuthenticationRequest {
    return AuthenticationRequest(
        username,
        password
    )
}