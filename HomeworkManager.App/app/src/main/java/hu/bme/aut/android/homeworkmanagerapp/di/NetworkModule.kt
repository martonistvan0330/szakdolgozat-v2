package hu.bme.aut.android.homeworkmanagerapp.di

import android.content.Context
import dagger.Module
import dagger.Provides
import dagger.hilt.InstallIn
import dagger.hilt.android.qualifiers.ApplicationContext
import dagger.hilt.components.SingletonComponent
import hu.bme.aut.android.homeworkmanagerapp.network.AuthInterceptor
import hu.bme.aut.android.homeworkmanagerapp.network.auth.AuthApi
import hu.bme.aut.android.homeworkmanagerapp.network.course.CourseApi
import hu.bme.aut.android.homeworkmanagerapp.network.group.GroupApi
import hu.bme.aut.android.homeworkmanagerapp.network.user.UserApi
import okhttp3.OkHttpClient
import okhttp3.logging.HttpLoggingInterceptor
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import javax.inject.Singleton

@Module
@InstallIn(SingletonComponent::class)
class NetworkModule {
    private val BASE_URL = "http://10.0.2.2:5020"

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
    fun provideUserApi(retrofit: Retrofit): UserApi {
        return retrofit.create(UserApi::class.java)
    }
}