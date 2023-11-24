package hu.bme.aut.android.homeworkmanagerapp.network

import retrofit2.Call
import retrofit2.Callback
import retrofit2.HttpException
import retrofit2.Response

suspend fun <T> handleRequest(block: suspend () -> T): T {
    return try {
        block()
    } catch (httpException: HttpException) {
        throw httpException
    } catch (exception: Exception) {
        println(exception.stackTrace)
        throw exception
    }
}

suspend fun <T> handleAuthorizedRequest(block: suspend () -> T): T {
    return try {
        block()
    } catch (httpException: HttpException) {
        if (httpException.code() == 401) {
            val authenticateService = AuthenticateService.create()

            return try {
                authenticateService.authenticate()
                handleRequest { block() }
            } catch (exception: Exception) {
                throw exception
            }
        } else {
            throw httpException
        }
    } catch (exception: Exception) {
        println(exception.stackTrace)
        throw exception
    }
}

fun <T> Call<T>?.handle(onSuccess: (T) -> Unit, onError: () -> Unit) {
    this?.enqueue(object : Callback<T> {
        override fun onResponse(
            call: Call<T>,
            response: Response<T>,
        ) {
            if (response.isSuccessful) {
                if (response.body() != null) {
                    onSuccess(response.body()!!)
                }
            } else {
                onError()
            }
        }

        override fun onFailure(
            call: Call<T>,
            throwable: Throwable,
        ) {
            throwable.printStackTrace()
            onError()
        }
    })
}

fun <T> Call<T>?.handleAuthorize(onSuccess: (T) -> Unit, onError: () -> Unit) {
    this?.enqueue(object : Callback<T> {
        override fun onResponse(
            call: Call<T>,
            response: Response<T>,
        ) {
            if (response.isSuccessful) {
                if (response.body() != null) {
                    onSuccess(response.body()!!)
                }
            } else {
                onError()
            }
        }

        override fun onFailure(
            call: Call<T>,
            throwable: Throwable,
        ) {
            throwable.printStackTrace()
            onError()
        }
    })
}