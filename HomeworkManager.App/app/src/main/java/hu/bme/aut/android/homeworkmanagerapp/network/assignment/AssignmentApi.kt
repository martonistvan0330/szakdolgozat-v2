package hu.bme.aut.android.homeworkmanagerapp.network.assignment

import hu.bme.aut.android.homeworkmanagerapp.domain.model.Pageable
import hu.bme.aut.android.homeworkmanagerapp.domain.model.assignment.AssignmentListRow
import retrofit2.http.GET

interface AssignmentApi {
    @GET("/api/Assignment")
    suspend fun getAssignments(): Pageable<AssignmentListRow>
}