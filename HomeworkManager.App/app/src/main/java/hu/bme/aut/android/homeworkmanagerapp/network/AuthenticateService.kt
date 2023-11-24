package hu.bme.aut.android.homeworkmanagerapp.network

import hu.bme.aut.android.homeworkmanagerapp.domain.model.user.UserModel
import okhttp3.OkHttpClient
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import retrofit2.http.GET

interface AuthenticateService {
    @GET("api/User/Authenticate")
    suspend fun authenticate(): UserModel

    companion object {
        private const val BASE_URL = "http://10.0.2.2:5020"

        fun create(): AuthenticateService {
            val client = OkHttpClient.Builder().build()

            return Retrofit.Builder()
                .baseUrl(BASE_URL)
                .client(client)
                .addConverterFactory(GsonConverterFactory.create())
                .build()
                .create(AuthenticateService::class.java)
        }
    }
}