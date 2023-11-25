package hu.bme.aut.android.homeworkmanagerapp.feature.group

import hu.bme.aut.android.homeworkmanagerapp.domain.model.group.GroupListRow
import hu.bme.aut.android.homeworkmanagerapp.network.RefreshService
import hu.bme.aut.android.homeworkmanagerapp.network.group.GroupNetworkManager
import hu.bme.aut.android.homeworkmanagerapp.network.handleAuthorizedRequest
import retrofit2.HttpException
import javax.inject.Inject

class GroupHandler @Inject constructor(
    private val groupNetworkManager: GroupNetworkManager,
    private val refreshService: RefreshService
) {
    suspend fun getGroups(
        courseId: Int,
        onSuccess: (List<GroupListRow>) -> Unit,
        onError: () -> Unit
    ) {
        try {
            val groups = handleAuthorizedRequest(refreshService) {
                groupNetworkManager.getGroups(courseId)
            }

            onSuccess(groups)
        } catch (httpException: HttpException) {
            onError()
        }
    }
}