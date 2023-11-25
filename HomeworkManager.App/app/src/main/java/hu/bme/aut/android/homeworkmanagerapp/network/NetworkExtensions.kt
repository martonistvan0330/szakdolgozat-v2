package hu.bme.aut.android.homeworkmanagerapp.network

import retrofit2.HttpException

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

suspend fun <T> handleAuthorizedRequest(
    refreshService: RefreshService,
    block: suspend () -> T
): T {
    return try {
        block()
    } catch (httpException: HttpException) {
        if (httpException.code() == 401) {
            return try {
                refreshService.refreshToken()
                handleRequest { block() }
            } catch (exception: Exception) {
                throw exception
            }
        } else {
            throw httpException
        }
    } catch (exception: Exception) {
        exception.printStackTrace()
        throw exception
    }
}