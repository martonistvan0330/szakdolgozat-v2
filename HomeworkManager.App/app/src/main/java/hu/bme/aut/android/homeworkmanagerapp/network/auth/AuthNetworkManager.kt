package hu.bme.aut.android.homeworkmanagerapp.network.auth

import hu.bme.aut.android.homeworkmanagerapp.domain.model.auth.AuthenticationRequest
import hu.bme.aut.android.homeworkmanagerapp.domain.model.auth.AuthenticationResponse
import hu.bme.aut.android.homeworkmanagerapp.domain.model.user.NewUser
import retrofit2.Call
import javax.inject.Inject

class AuthNetworkManager @Inject constructor(
    private val authApi: AuthApi
) {
    fun register(newUser: NewUser): Call<NewUser>? {
        return authApi.register(newUser)
    }

    fun login(authRequest: AuthenticationRequest): Call<AuthenticationResponse>? {
        return authApi.login(authRequest)
    }
}