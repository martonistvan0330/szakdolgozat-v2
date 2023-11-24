package hu.bme.aut.android.homeworkmanagerapp.network.auth

import hu.bme.aut.android.homeworkmanagerapp.domain.model.auth.AuthenticationRequest
import hu.bme.aut.android.homeworkmanagerapp.domain.model.auth.AuthenticationResponse
import hu.bme.aut.android.homeworkmanagerapp.domain.model.user.NewUser
import retrofit2.Call
import retrofit2.http.Body
import retrofit2.http.POST

interface AuthApi {
    @POST("/api/Auth/Register")
    suspend fun register(
        @Body newUser: NewUser,
    ): Call<NewUser>?

    @POST("/api/Auth/Login")
    suspend fun login(
        @Body authRequest: AuthenticationRequest,
    ): AuthenticationResponse
}