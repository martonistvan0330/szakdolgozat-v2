package hu.bme.aut.android.homeworkmanagerapp.network

import retrofit2.Call
import retrofit2.Callback
import retrofit2.Response

fun<T> Call<T>?.handle(onSuccess: (T) -> Unit, onError: () -> Unit) {
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

fun<T> Call<T>?.handleAuthorize(onSuccess: (T) -> Unit, onError: () -> Unit) {
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