package hu.bme.aut.android.homeworkmanagerapp.network.assignment

import hu.bme.aut.android.homeworkmanagerapp.domain.model.Pageable
import hu.bme.aut.android.homeworkmanagerapp.domain.model.assignment.AssignmentListRow
import retrofit2.http.GET
import retrofit2.http.Query

interface AssignmentApi {
    @GET("/api/Assignment")
    suspend fun getAssignments(
        @Query("pageData.pageIndex") pageIndex: Int,
        @Query("pageData.pageSize") pageSize: Int,
        @Query("searchText") searchText: String,
    ): Pageable<AssignmentListRow>
}