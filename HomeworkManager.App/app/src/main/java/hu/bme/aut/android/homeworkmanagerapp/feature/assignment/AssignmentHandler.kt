package hu.bme.aut.android.homeworkmanagerapp.feature.assignment

import androidx.paging.Pager
import androidx.paging.PagingConfig
import androidx.paging.PagingData
import hu.bme.aut.android.homeworkmanagerapp.domain.model.assignment.AssignmentListRowWithDate
import hu.bme.aut.android.homeworkmanagerapp.network.RefreshService
import hu.bme.aut.android.homeworkmanagerapp.network.assignment.AssignmentNetworkManager
import kotlinx.coroutines.flow.Flow
import javax.inject.Inject

class AssignmentHandler @Inject constructor(
    private val assignmentNetworkManager: AssignmentNetworkManager,
    private val refreshService: RefreshService
) {
    fun getAssignments(): Flow<PagingData<AssignmentListRowWithDate>> {
        return Pager(
            config = PagingConfig(enablePlaceholders = false, pageSize = 10),
            pagingSourceFactory = { assignmentNetworkManager }
        ).flow
    }

    // TODO delete
//    suspend fun getAssignments(
//        onSuccess: (List<AssignmentListRowWithDate>) -> Unit,
//        onError: () -> Unit
//    ) {
//        try {
//            val assignments = handleAuthorizedRequest(refreshService) {
//                assignmentNetworkManager.getAssignments()
//            }
//
//            onSuccess(assignments.items)
//        } catch (httpException: HttpException) {
//            onError()
//        }
//    }
}