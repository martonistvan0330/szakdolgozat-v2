package hu.bme.aut.android.homeworkmanagerapp.ui.model.group

import hu.bme.aut.android.homeworkmanagerapp.domain.model.group.GroupListRow

data class GroupListRowUi(
    val groupId: Int = 0,
    val name: String = "",
)

fun GroupListRow.asGroupListRowUi(): GroupListRowUi = GroupListRowUi(
    groupId = groupId,
    name = name,
)

fun GroupListRowUi.asGroupListRow(): GroupListRow = GroupListRow(
    groupId = groupId,
    name = name
)