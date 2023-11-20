package hu.bme.aut.android.homeworkmanagerapp.network.course

import android.content.Context
import hu.bme.aut.android.homeworkmanagerapp.domain.model.course.CourseListRow
import hu.bme.aut.android.homeworkmanagerapp.network.AuthInterceptor
import okhttp3.OkHttpClient
import retrofit2.Call
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

class CourseNetworkManager(context: Context) {
    private val retrofit: Retrofit
    private val courseApi: CourseApi

    private val SERVICE_URL = "http://10.0.2.2:5024"

    init {
        val client = OkHttpClient.Builder().addInterceptor(AuthInterceptor(context)).build()
        retrofit = Retrofit.Builder()
            .baseUrl(SERVICE_URL)
            .client(client)
            .addConverterFactory(GsonConverterFactory.create())
            .build()
        courseApi = retrofit.create(CourseApi::class.java)
    }

    fun getCourses(): Call<Array<CourseListRow>>? {
        return courseApi.getCourses()
    }
}