package hu.bme.aut.android.homeworkmanagerapp.feature.auth

import android.content.Context
import dagger.hilt.android.qualifiers.ApplicationContext
import hu.bme.aut.android.homeworkmanagerapp.domain.model.auth.AuthenticationRequest
import hu.bme.aut.android.homeworkmanagerapp.domain.model.user.NewUser
import hu.bme.aut.android.homeworkmanagerapp.network.auth.AuthNetworkManager
import hu.bme.aut.android.homeworkmanagerapp.network.handle
import javax.inject.Inject

class AuthHandler @Inject constructor(
    private val authNetworkManager: AuthNetworkManager,
    @ApplicationContext private val context: Context
) {
    fun register(
        firstName: String,
        lastName: String,
        email: String,
        username: String,
        password: String,
        onSuccess: () -> Unit,
        onError: () -> Unit
    ) {
        authNetworkManager.register(
            NewUser(
                firstName = firstName,
                lastName = lastName,
                username = username,
                password = password,
                email = email,
            )
        )?.handle(
            { onSuccess() },
            { onError() },
        )
    }

    fun login(username: String, password: String, onSuccess: () -> Unit, onError: () -> Unit) {
        authNetworkManager.login(AuthenticationRequest(username, password))?.handle(
            { authResponse ->
                val sharedPref = context.getSharedPreferences("auth", Context.MODE_PRIVATE)
                with(sharedPref.edit()) {
                    putString("access-token", authResponse.accessToken)
                    putString("refresh-token", authResponse.refreshToken)
                    apply()
                }
                onSuccess()
            },
            { onError() },
        )
    }
}