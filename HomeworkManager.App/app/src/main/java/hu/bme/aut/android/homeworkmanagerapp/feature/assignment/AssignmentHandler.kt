package hu.bme.aut.android.homeworkmanagerapp.feature.assignment

import hu.bme.aut.android.homeworkmanagerapp.domain.model.assignment.AssignmentListRowWithDate
import hu.bme.aut.android.homeworkmanagerapp.network.RefreshService
import hu.bme.aut.android.homeworkmanagerapp.network.assignment.AssignmentNetworkManager
import hu.bme.aut.android.homeworkmanagerapp.network.handleAuthorizedRequest
import retrofit2.HttpException
import javax.inject.Inject

class AssignmentHandler @Inject constructor(
    private val assignmentNetworkManager: AssignmentNetworkManager,
    private val refreshService: RefreshService
) {
    suspend fun getAssignments(
        onSuccess: (List<AssignmentListRowWithDate>) -> Unit,
        onError: () -> Unit
    ) {
        try {
            val assignments = handleAuthorizedRequest(refreshService) {
                assignmentNetworkManager.getAssignments()
            }

            onSuccess(assignments.items)
        } catch (httpException: HttpException) {
            onError()
        }
    }
}