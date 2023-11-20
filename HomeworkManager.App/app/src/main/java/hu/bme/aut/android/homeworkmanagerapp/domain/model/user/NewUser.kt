package hu.bme.aut.android.homeworkmanagerapp.domain.model.user

data class NewUser(
    val firstName: String,
    val lastName: String,
    val username: String,
    val password: String,
    val email: String,
)