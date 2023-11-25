package hu.bme.aut.android.homeworkmanagerapp.feature

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import javax.inject.Inject

@HiltViewModel
class HomeworkManagerAppViewModel @Inject constructor(
    private val authenticateService: AuthenticateService
) : ViewModel() {
    private val _loggedIn = MutableStateFlow(false)
    val loggedIn: StateFlow<Boolean> = _loggedIn.asStateFlow()

    init {
        authenticate()
    }

    private fun authenticate() {
        viewModelScope.launch {
            try {
                authenticateService.authenticate()
                _loggedIn.value = true
            } catch (exception: Exception) {
                exception.printStackTrace()
                _loggedIn.value = false
            }
        }
    }
}