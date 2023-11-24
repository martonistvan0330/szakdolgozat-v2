package hu.bme.aut.android.homeworkmanagerapp.network

import android.content.Context
import dagger.hilt.android.qualifiers.ApplicationContext
import hu.bme.aut.android.homeworkmanagerapp.domain.model.auth.RefreshRequest
import hu.bme.aut.android.homeworkmanagerapp.network.auth.AuthNetworkManager
import javax.inject.Inject

class RefreshService @Inject constructor(
    private val authNetworkManager: AuthNetworkManager,
    @ApplicationContext private val context: Context
) {
    suspend fun refreshToken() {
        val sharedPreferences = context.getSharedPreferences("auth", Context.MODE_PRIVATE)
        val refreshRequest = with(sharedPreferences) {
            val accessToken = getString("access-token", "") ?: ""
            val refreshToken = getString("refresh-token", "") ?: ""

            RefreshRequest(accessToken, refreshToken)
        }

        val authenticationResponse = authNetworkManager.refreshToken(refreshRequest)
        with(sharedPreferences.edit()) {
            putString("access-token", authenticationResponse.accessToken)
            putString("refresh-token", authenticationResponse.refreshToken)
            apply()
        }
    }
}