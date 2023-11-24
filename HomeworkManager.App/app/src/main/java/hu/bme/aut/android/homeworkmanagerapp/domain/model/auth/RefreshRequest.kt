package hu.bme.aut.android.homeworkmanagerapp.domain.model.auth

data class RefreshRequest(
    val accessToken: String,
    val refreshToken: String
)
