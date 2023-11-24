package hu.bme.aut.android.homeworkmanagerapp.ui.model.auth

import hu.bme.aut.android.homeworkmanagerapp.domain.model.user.NewUser

data class RegisterUiState(
    val firstName: String = "",
    val isFirstNameError: Boolean = false,
    val lastName: String = "",
    val isLastNameError: Boolean = false,
    val email: String = "",
    val isEmailError: Boolean = false,
    val username: String = "",
    val isUsernameError: Boolean = false,
    val password: String = "",
    val isPasswordError: Boolean = false,
    val isPasswordVisible: Boolean = false,
    val confirmPassword: String = "",
    val isConfirmPasswordError: Boolean = false,
    val isConfirmPasswordVisible: Boolean = false,
)

fun RegisterUiState.asNewUser(): NewUser {
    return NewUser(
        firstName = firstName,
        lastName = lastName,
        email = email,
        username = username,
        password = password
    )
}