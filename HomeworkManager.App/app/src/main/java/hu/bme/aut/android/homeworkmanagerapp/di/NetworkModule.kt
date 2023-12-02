package hu.bme.aut.android.homeworkmanagerapp.di

import android.content.Context
import com.microsoft.signalr.HubConnection
import com.microsoft.signalr.HubConnectionBuilder
import dagger.Module
import dagger.Provides
import dagger.hilt.InstallIn
import dagger.hilt.android.qualifiers.ApplicationContext
import dagger.hilt.components.SingletonComponent
import hu.bme.aut.android.homeworkmanagerapp.network.AuthInterceptor
import hu.bme.aut.android.homeworkmanagerapp.network.appointment.AppointmentApi
import hu.bme.aut.android.homeworkmanagerapp.network.assignment.AssignmentApi
import hu.bme.aut.android.homeworkmanagerapp.network.auth.AuthApi
import hu.bme.aut.android.homeworkmanagerapp.network.course.CourseApi
import hu.bme.aut.android.homeworkmanagerapp.network.group.GroupApi
import hu.bme.aut.android.homeworkmanagerapp.network.submission.SubmissionApi
import hu.bme.aut.android.homeworkmanagerapp.network.user.UserApi
import okhttp3.OkHttpClient
import okhttp3.logging.HttpLoggingInterceptor
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import javax.inject.Singleton

@Module
@InstallIn(SingletonComponent::class)
class NetworkModule {
    @Singleton
    @Provides
    fun provideAuthInterceptor(@ApplicationContext context: Context): AuthInterceptor {
        return AuthInterceptor(context)
    }

    @Singleton
    @Provides
    fun provideClient(authInterceptor: AuthInterceptor): OkHttpClient {
        val logger = HttpLoggingInterceptor().apply { level = HttpLoggingInterceptor.Level.BASIC }

        return OkHttpClient.Builder()
            .addInterceptor(authInterceptor)
            .addInterceptor(logger)
            .build()
    }

    @Singleton
    @Provides
    fun provideRetrofit(client: OkHttpClient): Retrofit {
        return Retrofit.Builder()
            .baseUrl(BASE_URL)
            .client(client)
            .addConverterFactory(GsonConverterFactory.create())
            .build()
    }

    @Provides
    fun provideAppointmentApi(retrofit: Retrofit): AppointmentApi {
        return retrofit.create(AppointmentApi::class.java)
    }

    @Provides
    fun provideAssignmentApi(retrofit: Retrofit): AssignmentApi {
        return retrofit.create(AssignmentApi::class.java)
    }

    @Provides
    fun provideAuthApi(retrofit: Retrofit): AuthApi {
        return retrofit.create(AuthApi::class.java)
    }

    @Provides
    fun provideCourseApi(retrofit: Retrofit): CourseApi {
        return retrofit.create(CourseApi::class.java)
    }

    @Provides
    fun provideGroupApi(retrofit: Retrofit): GroupApi {
        return retrofit.create(GroupApi::class.java)
    }

    @Provides
    fun provideSubmissionApi(retrofit: Retrofit): SubmissionApi {
        return retrofit.create(SubmissionApi::class.java)
    }

    @Provides
    fun provideUserApi(retrofit: Retrofit): UserApi {
        return retrofit.create(UserApi::class.java)
    }

    @Provides
    fun provideAppointmentHubConnection(@ApplicationContext context: Context): HubConnection {
        return HubConnectionBuilder
            .create("$BASE_URL/appointment")
//            .withAccessTokenProvider(Single.create {
//                val sharedPref = context.getSharedPreferences("auth", Context.MODE_PRIVATE)
//                sharedPref.getString("access-token", "")
//            })
            .build()
    }

    companion object {
        const val BASE_URL = "http://10.0.2.2:5020"
    }
}