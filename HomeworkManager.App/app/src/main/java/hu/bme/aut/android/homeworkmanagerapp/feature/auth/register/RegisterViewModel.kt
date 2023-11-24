package hu.bme.aut.android.homeworkmanagerapp.feature.auth.register

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import dagger.hilt.android.lifecycle.HiltViewModel
import hu.bme.aut.android.homeworkmanagerapp.domain.model.user.NewUser
import hu.bme.aut.android.homeworkmanagerapp.feature.auth.AuthHandler
import hu.bme.aut.android.homeworkmanagerapp.ui.model.auth.RegisterUiState
import hu.bme.aut.android.homeworkmanagerapp.ui.model.auth.asNewUser
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.update
import kotlinx.coroutines.launch
import javax.inject.Inject

@HiltViewModel
class RegisterViewModel @Inject constructor(
    private val authHandler: AuthHandler
) : ViewModel() {
    private val _registerUiState = MutableStateFlow(RegisterUiState())
    val registerUiState: StateFlow<RegisterUiState> = _registerUiState.asStateFlow()

    fun updateFirstName(newValue: String) {
        _registerUiState.update { currentState ->
            currentState.copy(
                firstName = newValue,
                isFirstNameError = false
            )
        }
    }

    fun updateLastName(newValue: String) {
        _registerUiState.update { currentState ->
            currentState.copy(
                lastName = newValue,
                isLastNameError = false
            )
        }
    }

    fun updateEmail(newValue: String) {
        _registerUiState.update { currentState ->
            currentState.copy(
                email = newValue,
                isEmailError = false
            )
        }
    }

    fun updateUsername(newValue: String) {
        _registerUiState.update { currentState ->
            currentState.copy(
                username = newValue,
                isUsernameError = false
            )
        }
    }

    fun updatePassword(newValue: String) {
        _registerUiState.update { currentState ->
            currentState.copy(
                password = newValue,
                isPasswordError = false
            )
        }
    }

    fun changePasswordVisibility() {
        _registerUiState.update { currentState ->
            currentState.copy(
                isPasswordVisible = !currentState.isPasswordVisible,
            )
        }
    }

    fun updateConfirmPassword(newValue: String) {
        _registerUiState.update { currentState ->
            currentState.copy(
                confirmPassword = newValue,
                isConfirmPasswordError = false
            )
        }
    }

    fun changeConfirmPasswordVisibility() {
        _registerUiState.update { currentState ->
            currentState.copy(
                isConfirmPasswordVisible = !currentState.isConfirmPasswordVisible,
            )
        }
    }

    fun register(
        onRegister: () -> Unit,
        onError: () -> Unit
    ) {
        val state = registerUiState.value

        if (state.email.isEmpty() || !state.email.contains(Regex("^[\\w-.]+@([\\w-]+\\.)+[\\w-]{2,4}\$"))) {
            _registerUiState.update { currentState ->
                currentState.copy(
                    isEmailError = true
                )
            }
        } else if (state.username.isEmpty()) {
            _registerUiState.update { currentState ->
                currentState.copy(
                    isUsernameError = true
                )
            }
        } else if (state.password.isEmpty()) {
            _registerUiState.update { currentState ->
                currentState.copy(
                    isPasswordError = true
                )
            }
        } else if (state.confirmPassword.isEmpty() || state.password != state.confirmPassword) {
            _registerUiState.update { currentState ->
                currentState.copy(
                    isConfirmPasswordError = true
                )
            }
        } else {
            register(
                newUser = state.asNewUser(),
                onRegister = onRegister,
                onError = onError,
            )
        }
    }

    private fun register(
        newUser: NewUser,
        onRegister: () -> Unit,
        onError: () -> Unit
    ) {
        viewModelScope.launch {
            authHandler.register(newUser, onRegister, onError)
        }
    }
}