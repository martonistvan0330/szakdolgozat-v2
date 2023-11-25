package hu.bme.aut.android.homeworkmanagerapp.network.group

import hu.bme.aut.android.homeworkmanagerapp.domain.model.Pageable
import hu.bme.aut.android.homeworkmanagerapp.domain.model.assignment.AssignmentListRow
import hu.bme.aut.android.homeworkmanagerapp.domain.model.group.GroupListRow
import retrofit2.http.GET
import retrofit2.http.Path
import retrofit2.http.Query

interface GroupApi {
    @GET("/api/Course/{courseId}/Group")
    suspend fun getGroups(
        @Path("courseId") courseId: Int
    ): List<GroupListRow>

    @GET("/api/Course/{courseId}/Group/{groupName}/Assignment")
    suspend fun getAssignments(
        @Path("courseId") courseId: Int,
        @Path("groupName") groupName: String,
        @Query("pageData.pageIndex") pageIndex: Int,
        @Query("pageData.pageSize") pageSize: Int,
        @Query("searchText") searchText: String,
    ): Pageable<AssignmentListRow>
}