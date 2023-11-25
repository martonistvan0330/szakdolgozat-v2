package hu.bme.aut.android.homeworkmanagerapp.network.assignment

import androidx.paging.PagingSource
import androidx.paging.PagingState
import hu.bme.aut.android.homeworkmanagerapp.domain.model.assignment.withDate
import hu.bme.aut.android.homeworkmanagerapp.ui.model.assignment.AssignmentListRowUi
import hu.bme.aut.android.homeworkmanagerapp.ui.model.assignment.asAssignmentListRowUi
import javax.inject.Inject
import kotlin.math.ceil

class AssignmentNetworkManager @Inject constructor(
    private val searchText: String,
    private val assignmentApi: AssignmentApi
) : PagingSource<Int, AssignmentListRowUi>() {
    override suspend fun load(params: LoadParams<Int>): LoadResult<Int, AssignmentListRowUi> {
        val page = params.key ?: 0
        return try {
            val assignments = assignmentApi.getAssignments(page, params.loadSize, searchText)
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

// TODO delete
//    suspend fun getAssignments(): Pageable<AssignmentListRowWithDate> {
//        val assignments = assignmentApi.getAssignments()
//
//        return Pageable(
//            items = assignments.items.map { it.withDate() },
//            totalCount = assignments.totalCount
//        )
//    }
}