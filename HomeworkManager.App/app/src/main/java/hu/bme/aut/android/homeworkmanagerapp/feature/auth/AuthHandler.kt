package hu.bme.aut.android.homeworkmanagerapp.feature.auth

import android.content.Context
import dagger.hilt.android.qualifiers.ApplicationContext
import hu.bme.aut.android.homeworkmanagerapp.domain.model.auth.AuthenticationRequest
import hu.bme.aut.android.homeworkmanagerapp.domain.model.user.NewUser
import hu.bme.aut.android.homeworkmanagerapp.network.auth.AuthNetworkManager
import hu.bme.aut.android.homeworkmanagerapp.network.handle
import retrofit2.HttpException
import javax.inject.Inject

class AuthHandler @Inject constructor(
    private val authNetworkManager: AuthNetworkManager,
    @ApplicationContext private val context: Context
) {
    /*suspend*/ fun register(
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
        ).handle(
            { onSuccess() },
            { onError() },
        )
    }

    suspend fun login(
        authenticationRequest: AuthenticationRequest,
        onSuccess: () -> Unit,
        onError: () -> Unit
    ) {
        try {
            val authenticationResponse = authNetworkManager.login(authenticationRequest)
            val sharedPreferences = context.getSharedPreferences("auth", Context.MODE_PRIVATE)
            with(sharedPreferences.edit()) {
                putString("access-token", authenticationResponse.accessToken)
                putString("refresh-token", authenticationResponse.refreshToken)
                apply()
            }
            onSuccess()
        } catch (httpException: HttpException) {
            onError()
        } catch (exception: Exception) {
            println(exception.stackTrace)
        }
    }
}