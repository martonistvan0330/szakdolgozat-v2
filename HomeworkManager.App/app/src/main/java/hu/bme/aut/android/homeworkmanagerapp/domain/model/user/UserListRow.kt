package hu.bme.aut.android.homeworkmanagerapp.domain.model.user

data class UserListRow(
    val userId: String,
    val fullName: String,
    val username: String,
    val email: String,
    val roles: String,
)