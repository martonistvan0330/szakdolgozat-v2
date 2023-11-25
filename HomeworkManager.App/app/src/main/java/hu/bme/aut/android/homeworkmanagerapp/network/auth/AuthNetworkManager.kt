package hu.bme.aut.android.homeworkmanagerapp.network.auth

import hu.bme.aut.android.homeworkmanagerapp.domain.model.auth.AuthenticationRequest
import hu.bme.aut.android.homeworkmanagerapp.domain.model.auth.AuthenticationResponse
import hu.bme.aut.android.homeworkmanagerapp.domain.model.auth.RefreshRequest
import hu.bme.aut.android.homeworkmanagerapp.domain.model.auth.RevokeRequest
import hu.bme.aut.android.homeworkmanagerapp.domain.model.user.NewUser
import javax.inject.Inject

class AuthNetworkManager @Inject constructor(
    private val authApi: AuthApi
) {
    suspend fun register(newUser: NewUser): AuthenticationResponse {
        return authApi.register(newUser)
    }

    suspend fun login(authRequest: AuthenticationRequest): AuthenticationResponse {
        return authApi.login(authRequest)
    }

    suspend fun refreshToken(refreshRequest: RefreshRequest): AuthenticationResponse {
        return authApi.refreshToken(refreshRequest)
    }

    suspend fun logout(revokeRequest: RevokeRequest): Boolean {
        return authApi.logout(revokeRequest)
    }
}