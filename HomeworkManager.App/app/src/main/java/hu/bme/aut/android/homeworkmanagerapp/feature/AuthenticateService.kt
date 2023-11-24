package hu.bme.aut.android.homeworkmanagerapp.feature

import hu.bme.aut.android.homeworkmanagerapp.network.RefreshService
import hu.bme.aut.android.homeworkmanagerapp.network.handleAuthorizedRequest
import hu.bme.aut.android.homeworkmanagerapp.network.user.UserNetworkManager
import javax.inject.Inject

class AuthenticateService @Inject constructor(
    private val userNetworkManager: UserNetworkManager,
    private val refreshService: RefreshService
) {
    suspend fun authenticate() {
        handleAuthorizedRequest(refreshService) {
            userNetworkManager.authenticate()
        }
    }
}