package hu.bme.aut.android.homeworkmanagerapp.network.group

import hu.bme.aut.android.homeworkmanagerapp.domain.model.group.GroupListRow
import retrofit2.http.GET
import retrofit2.http.Path

interface GroupApi {
    @GET("/api/Course/{courseId}/Group")
    suspend fun getGroups(
        @Path("courseId") courseId: Int
    ): List<GroupListRow>
}