package hu.bme.aut.android.homeworkmanagerapp.feature.group.details.teacher

import androidx.paging.PagingSource
import androidx.paging.PagingState
import hu.bme.aut.android.homeworkmanagerapp.network.group.GroupNetworkManager
import hu.bme.aut.android.homeworkmanagerapp.ui.model.user.UserListRowUi
import hu.bme.aut.android.homeworkmanagerapp.ui.model.user.asUserListRowUi
import kotlin.math.ceil

class GroupTeacherPagingSource(
    private val courseId: Int,
    private val groupName: String,
    private val searchText: String,
    private val groupNetworkManager: GroupNetworkManager
) : PagingSource<Int, UserListRowUi>() {
    override suspend fun load(params: LoadParams<Int>): LoadResult<Int, UserListRowUi> {
        val page = params.key ?: 0
        return try {
            val teachers =
                groupNetworkManager.getTeachers(
                    courseId,
                    groupName,
                    page,
                    params.loadSize,
                    searchText
                )
            val maxPageIndex = ceil(teachers.totalCount / params.loadSize.toDouble()).toInt() - 1
            LoadResult.Page(
                data = teachers.items.map { it.asUserListRowUi() },
                prevKey = if (page == 0) null else page - 1,
                nextKey = if (page == maxPageIndex) null else page + 1
            )
        } catch (exception: Exception) {
            LoadResult.Error(exception)
        }
    }

    override fun getRefreshKey(state: PagingState<Int, UserListRowUi>): Int? {
        return state.anchorPosition?.let { anchorPosition ->
            state.closestPageToPosition(anchorPosition)?.prevKey
        }
    }
}