package hu.bme.aut.android.homeworkmanagerapp.feature.group

import androidx.paging.Pager
import androidx.paging.PagingConfig
import androidx.paging.PagingData
import hu.bme.aut.android.homeworkmanagerapp.domain.model.group.GroupListRow
import hu.bme.aut.android.homeworkmanagerapp.feature.group.details.student.GroupStudentPagingSource
import hu.bme.aut.android.homeworkmanagerapp.feature.group.details.teacher.GroupTeacherPagingSource
import hu.bme.aut.android.homeworkmanagerapp.network.RefreshService
import hu.bme.aut.android.homeworkmanagerapp.network.group.GroupNetworkManager
import hu.bme.aut.android.homeworkmanagerapp.network.handleAuthorizedRequest
import hu.bme.aut.android.homeworkmanagerapp.ui.model.user.UserListRowUi
import kotlinx.coroutines.flow.Flow
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

    fun getTeachers(
        courseId: Int,
        groupName: String,
        searchText: String
    ): Flow<PagingData<UserListRowUi>> {
        return Pager(
            config = PagingConfig(enablePlaceholders = false, pageSize = 25),
            pagingSourceFactory = {
                GroupTeacherPagingSource(
                    courseId,
                    groupName,
                    searchText,
                    groupNetworkManager
                )
            }
        ).flow
    }

    fun getStudents(
        courseId: Int,
        groupName: String,
        searchText: String
    ): Flow<PagingData<UserListRowUi>> {
        return Pager(
            config = PagingConfig(enablePlaceholders = false, pageSize = 25),
            pagingSourceFactory = {
                GroupStudentPagingSource(
                    courseId,
                    groupName,
                    searchText,
                    groupNetworkManager
                )
            }
        ).flow
    }
}