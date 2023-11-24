package hu.bme.aut.android.homeworkmanagerapp.ui.model.auth

import hu.bme.aut.android.homeworkmanagerapp.domain.model.auth.AuthenticationRequest

data class LoginUiState(
    var username: String = "",
    var isUsernameError: Boolean = false,
    var password: String = "",
    var isPasswordError: Boolean = false,
    var isPasswordVisible: Boolean = false
)

fun LoginUiState.asAuthenticationRequest(): AuthenticationRequest {
    return AuthenticationRequest(
        username,
        password
    )
}