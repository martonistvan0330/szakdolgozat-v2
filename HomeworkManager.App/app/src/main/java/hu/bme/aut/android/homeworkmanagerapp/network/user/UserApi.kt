package hu.bme.aut.android.homeworkmanagerapp.network.user

import hu.bme.aut.android.homeworkmanagerapp.domain.model.user.UserModel
import retrofit2.http.GET

interface UserApi {
    @GET("/api/User/Authenticate")
    suspend fun authenticate(): UserModel
}