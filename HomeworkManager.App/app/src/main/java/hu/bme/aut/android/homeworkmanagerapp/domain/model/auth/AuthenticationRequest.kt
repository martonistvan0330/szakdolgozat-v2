package hu.bme.aut.android.homeworkmanagerapp.domain.model.auth

data class AuthenticationRequest(
    var username: String,
    var password: String,
)