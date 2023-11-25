package hu.bme.aut.android.homeworkmanagerapp.network.group

import hu.bme.aut.android.homeworkmanagerapp.domain.model.group.GroupListRow
import javax.inject.Inject

class GroupNetworkManager @Inject constructor(
    private val groupApi: GroupApi
) {
    suspend fun getGroups(courseId: Int): List<GroupListRow> {
        return groupApi.getGroups(courseId)
    }
}