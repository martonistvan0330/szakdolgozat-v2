package hu.bme.aut.android.homeworkmanagerapp.feature.auth

import android.content.Context
import dagger.hilt.android.qualifiers.ApplicationContext
import hu.bme.aut.android.homeworkmanagerapp.domain.model.auth.AuthenticationRequest
import hu.bme.aut.android.homeworkmanagerapp.domain.model.auth.RevokeRequest
import hu.bme.aut.android.homeworkmanagerapp.domain.model.user.NewUser
import hu.bme.aut.android.homeworkmanagerapp.network.auth.AuthNetworkManager
import hu.bme.aut.android.homeworkmanagerapp.network.handleRequest
import retrofit2.HttpException
import javax.inject.Inject

class AuthHandler @Inject constructor(
    private val authNetworkManager: AuthNetworkManager,
    @ApplicationContext private val context: Context
) {
    suspend fun register(
        newUser: NewUser,
        onSuccess: () -> Unit,
        onError: () -> Unit
    ) {
        try {
            val authenticationResponse = handleRequest {
                authNetworkManager.register(newUser)
            }

            val sharedPreferences = context.getSharedPreferences("auth", Context.MODE_PRIVATE)
            with(sharedPreferences.edit()) {
                putString("access-token", authenticationResponse.accessToken)
                putString("refresh-token", authenticationResponse.refreshToken)
                apply()
            }

            onSuccess()
        } catch (httpException: HttpException) {
            onError()
        }
    }

    suspend fun login(
        authenticationRequest: AuthenticationRequest,
        onSuccess: () -> Unit,
        onError: () -> Unit
    ) {
        try {
            val authenticationResponse = handleRequest {
                authNetworkManager.login(authenticationRequest)
            }

            val sharedPreferences = context.getSharedPreferences("auth", Context.MODE_PRIVATE)
            with(sharedPreferences.edit()) {
                putString("access-token", authenticationResponse.accessToken)
                putString("refresh-token", authenticationResponse.refreshToken)
                apply()
            }

            onSuccess()
        } catch (httpException: HttpException) {
            onError()
        }
    }

    suspend fun logout(
        onLogout: () -> Unit
    ) {
        val sharedPreferences = context.getSharedPreferences("auth", Context.MODE_PRIVATE)
        try {
            val revokeRequest = with(sharedPreferences) {
                val accessToken = getString("access-token", "") ?: ""
                val refreshToken = getString("refresh-token", "") ?: ""

                RevokeRequest(accessToken, refreshToken)
            }
            authNetworkManager.logout(revokeRequest)
        } catch (exception: Exception) {
            exception.printStackTrace()
        } finally {
            with(sharedPreferences.edit()) {
                remove("access-token")
                remove("refresh-token")
                apply()
            }
            onLogout()
        }
    }
}