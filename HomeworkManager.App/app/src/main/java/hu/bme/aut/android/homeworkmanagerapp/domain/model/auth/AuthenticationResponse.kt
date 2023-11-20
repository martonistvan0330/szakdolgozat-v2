package hu.bme.aut.android.homeworkmanagerapp.domain.model.auth

import java.util.Date

data class AuthenticationResponse(
    val accessToken: String,
    val refreshToken: String,
    val expiration: Date,
)
