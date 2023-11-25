package hu.bme.aut.android.homeworkmanagerapp.domain.model.auth

data class RevokeRequest(
    val accessToken: String,
    val refreshToken: String
)