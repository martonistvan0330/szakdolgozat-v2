package hu.bme.aut.android.homeworkmanagerapp.network.course

import hu.bme.aut.android.homeworkmanagerapp.domain.model.course.CourseListRow
import javax.inject.Inject

class CourseNetworkManager @Inject constructor(
    private val courseApi: CourseApi
) {
    suspend fun getCourses(): List<CourseListRow> {
        return courseApi.getCourses()
    }
}