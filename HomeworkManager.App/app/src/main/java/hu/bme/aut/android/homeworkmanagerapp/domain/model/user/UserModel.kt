package hu.bme.aut.android.homeworkmanagerapp.domain.model.user

data class UserModel(
    val userId: String,
    val fullName: String,
    val username: String,
    val email: String,
    val emailConfirmed: Boolean,
    val roles: List<RoleModel>
)