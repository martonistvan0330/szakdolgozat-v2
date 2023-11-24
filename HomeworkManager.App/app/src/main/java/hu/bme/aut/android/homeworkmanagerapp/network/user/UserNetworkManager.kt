package hu.bme.aut.android.homeworkmanagerapp.network.user

import hu.bme.aut.android.homeworkmanagerapp.domain.model.user.UserModel
import javax.inject.Inject

class UserNetworkManager @Inject constructor(
    private val userApi: UserApi
) {
    suspend fun authenticate(): UserModel {
        return userApi.authenticate()
    }
}