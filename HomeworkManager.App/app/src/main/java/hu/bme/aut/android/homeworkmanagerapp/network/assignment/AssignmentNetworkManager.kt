package hu.bme.aut.android.homeworkmanagerapp.network.assignment

import hu.bme.aut.android.homeworkmanagerapp.domain.model.Pageable
import hu.bme.aut.android.homeworkmanagerapp.domain.model.assignment.AssignmentListRowWithDate
import hu.bme.aut.android.homeworkmanagerapp.domain.model.assignment.withDate
import javax.inject.Inject

class AssignmentNetworkManager @Inject constructor(
    private val assignmentApi: AssignmentApi
) {
    suspend fun getAssignments(): Pageable<AssignmentListRowWithDate> {
        val assignments = assignmentApi.getAssignments()

        return Pageable(
            items = assignments.items.map { it.withDate() },
            totalCount = assignments.totalCount
        )
    }
}