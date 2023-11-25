package hu.bme.aut.android.homeworkmanagerapp.network.group

import hu.bme.aut.android.homeworkmanagerapp.domain.model.Pageable
import hu.bme.aut.android.homeworkmanagerapp.domain.model.group.GroupListRow
import hu.bme.aut.android.homeworkmanagerapp.domain.model.user.UserListRow
import javax.inject.Inject

class GroupNetworkManager @Inject constructor(
    private val groupApi: GroupApi
) {
    suspend fun getGroups(courseId: Int): List<GroupListRow> {
        return groupApi.getGroups(courseId)
    }

    suspend fun getTeachers(
        courseId: Int,
        groupName: String,
        pageIndex: Int,
        pageSize: Int,
        searchText: String,
    ): Pageable<UserListRow> {
        return groupApi.getTeachers(courseId, groupName, pageIndex, pageSize, searchText)
    }

    suspend fun getStudents(
        courseId: Int,
        groupName: String,
        pageIndex: Int,
        pageSize: Int,
        searchText: String,
    ): Pageable<UserListRow> {
        return groupApi.getStudents(courseId, groupName, pageIndex, pageSize, searchText)
    }
}