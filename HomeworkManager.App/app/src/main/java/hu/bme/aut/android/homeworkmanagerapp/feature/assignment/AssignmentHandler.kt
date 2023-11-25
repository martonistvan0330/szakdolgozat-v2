package hu.bme.aut.android.homeworkmanagerapp.feature.assignment

import androidx.paging.Pager
import androidx.paging.PagingConfig
import androidx.paging.PagingData
import hu.bme.aut.android.homeworkmanagerapp.feature.group.details.assignment.GroupAssignmentPagingSource
import hu.bme.aut.android.homeworkmanagerapp.network.RefreshService
import hu.bme.aut.android.homeworkmanagerapp.network.assignment.AssignmentApi
import hu.bme.aut.android.homeworkmanagerapp.network.assignment.AssignmentNetworkManager
import hu.bme.aut.android.homeworkmanagerapp.network.group.GroupApi
import hu.bme.aut.android.homeworkmanagerapp.ui.model.assignment.AssignmentListRowUi
import kotlinx.coroutines.flow.Flow
import javax.inject.Inject

class AssignmentHandler @Inject constructor(
    private val assignmentApi: AssignmentApi,
    private val groupApi: GroupApi,
    private val refreshService: RefreshService
) {
    fun getAssignments(searchText: String): Flow<PagingData<AssignmentListRowUi>> {
        return Pager(
            config = PagingConfig(enablePlaceholders = false, pageSize = 25),
            pagingSourceFactory = { AssignmentNetworkManager(searchText, assignmentApi) }
        ).flow
    }

    fun getAssignments(
        courseId: Int,
        groupName: String,
        searchText: String
    ): Flow<PagingData<AssignmentListRowUi>> {
        return Pager(
            config = PagingConfig(enablePlaceholders = false, pageSize = 25),
            pagingSourceFactory = {
                GroupAssignmentPagingSource(
                    courseId,
                    groupName,
                    searchText,
                    groupApi
                )
            }
        ).flow
    }
}