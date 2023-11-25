package hu.bme.aut.android.homeworkmanagerapp.feature.group.details.assignment

import androidx.paging.PagingSource
import androidx.paging.PagingState
import hu.bme.aut.android.homeworkmanagerapp.domain.model.assignment.withDate
import hu.bme.aut.android.homeworkmanagerapp.network.group.GroupApi
import hu.bme.aut.android.homeworkmanagerapp.ui.model.assignment.AssignmentListRowUi
import hu.bme.aut.android.homeworkmanagerapp.ui.model.assignment.asAssignmentListRowUi
import kotlin.math.ceil

class GroupAssignmentPagingSource(
    private val courseId: Int,
    private val groupName: String,
    private val searchText: String,
    private val groupApi: GroupApi
) : PagingSource<Int, AssignmentListRowUi>() {
    override suspend fun load(params: LoadParams<Int>): LoadResult<Int, AssignmentListRowUi> {
        val page = params.key ?: 0
        return try {
            val assignments =
                groupApi.getAssignments(courseId, groupName, page, params.loadSize, searchText)
            val maxPageIndex = ceil(assignments.totalCount / params.loadSize.toDouble()).toInt() - 1
            LoadResult.Page(
                data = assignments.items.map { it.withDate().asAssignmentListRowUi() },
                prevKey = if (page == 0) null else page - 1,
                nextKey = if (page == maxPageIndex) null else page + 1
            )
        } catch (exception: Exception) {
            LoadResult.Error(exception)
        }
    }

    override fun getRefreshKey(state: PagingState<Int, AssignmentListRowUi>): Int? {
        return state.anchorPosition?.let { anchorPosition ->
            state.closestPageToPosition(anchorPosition)?.prevKey
        }
    }
}