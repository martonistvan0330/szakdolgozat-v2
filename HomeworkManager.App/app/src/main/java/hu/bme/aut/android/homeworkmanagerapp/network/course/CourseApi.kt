package hu.bme.aut.android.homeworkmanagerapp.network.course

import hu.bme.aut.android.homeworkmanagerapp.domain.model.course.CourseListRow
import retrofit2.http.GET

interface CourseApi {
    @GET("/api/Course")
    suspend fun getCourses(): List<CourseListRow>
}