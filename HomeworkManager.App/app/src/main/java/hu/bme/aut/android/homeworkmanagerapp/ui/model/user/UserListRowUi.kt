package hu.bme.aut.android.homeworkmanagerapp.ui.model.user

import hu.bme.aut.android.homeworkmanagerapp.domain.model.user.UserListRow

class UserListRowUi(
    val userId: String,
    val fullName: String,
    val email: String,
)

fun UserListRow.asUserListRowUi(): UserListRowUi {
    return UserListRowUi(
        userId = userId,
        fullName = fullName,
        email = email,
    )
}