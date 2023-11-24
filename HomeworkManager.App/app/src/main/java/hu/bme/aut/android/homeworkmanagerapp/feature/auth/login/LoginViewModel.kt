package hu.bme.aut.android.homeworkmanagerapp.feature.auth.login

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import dagger.hilt.android.lifecycle.HiltViewModel
import hu.bme.aut.android.homeworkmanagerapp.domain.model.auth.AuthenticationRequest
import hu.bme.aut.android.homeworkmanagerapp.feature.auth.AuthHandler
import hu.bme.aut.android.homeworkmanagerapp.ui.model.auth.LoginUiState
import hu.bme.aut.android.homeworkmanagerapp.ui.model.auth.asAuthenticationRequest
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.update
import kotlinx.coroutines.launch
import javax.inject.Inject

@HiltViewModel
class LoginViewModel @Inject constructor(
    private val authHandler: AuthHandler
) : ViewModel() {
    private val _loginUiState = MutableStateFlow(LoginUiState())
    val loginUiState: StateFlow<LoginUiState> = _loginUiState.asStateFlow()

    fun updateUsername(newValue: String) {
        _loginUiState.update { currentState ->
            currentState.copy(
                username = newValue,
                isUsernameError = false
            )
        }
    }

    fun updatePassword(newValue: String) {
        _loginUiState.update { currentState ->
            currentState.copy(
                password = newValue,
                isPasswordError = false
            )
        }
    }

    fun changePasswordVisibility() {
        _loginUiState.update { currentState ->
            currentState.copy(
                isPasswordVisible = !currentState.isPasswordVisible,
            )
        }
    }

    fun login(onLogin: () -> Unit, onError: () -> Unit) {
        val state = loginUiState.value

        if (state.username.isEmpty()) {
            _loginUiState.update { currentState ->
                currentState.copy(
                    isUsernameError = true
                )
            }
        } else if (state.password.isEmpty()) {
            _loginUiState.update { currentState ->
                currentState.copy(
                    isPasswordError = true
                )
            }
        } else {
            login(loginUiState.value.asAuthenticationRequest(), onLogin, onError)
        }
    }

    private fun login(
        authenticationRequest: AuthenticationRequest,
        onLogin: () -> Unit,
        onError: () -> Unit
    ) {
        viewModelScope.launch {
            authHandler.login(authenticationRequest, onLogin, onError)
        }
    }
}