package hu.bme.aut.android.homeworkmanagerapp.network.auth

import hu.bme.aut.android.homeworkmanagerapp.domain.model.auth.AuthenticationRequest
import hu.bme.aut.android.homeworkmanagerapp.domain.model.auth.AuthenticationResponse
import hu.bme.aut.android.homeworkmanagerapp.domain.model.auth.RefreshRequest
import hu.bme.aut.android.homeworkmanagerapp.domain.model.auth.RevokeRequest
import hu.bme.aut.android.homeworkmanagerapp.domain.model.user.NewUser
import retrofit2.http.Body
import retrofit2.http.POST

interface AuthApi {
    @POST("/api/Auth/Register")
    suspend fun register(
        @Body newUser: NewUser,
    ): AuthenticationResponse

    @POST("/api/Auth/Login")
    suspend fun login(
        @Body authRequest: AuthenticationRequest,
    ): AuthenticationResponse

    @POST("/api/Auth/RefreshToken")
    suspend fun refreshToken(
        @Body refreshRequest: RefreshRequest,
    ): AuthenticationResponse

    @POST("/api/Auth/Logout")
    suspend fun logout(
        @Body revokeRequest: RevokeRequest
    ): Boolean
}