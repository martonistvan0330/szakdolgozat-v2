package hu.bme.aut.android.homeworkmanagerapp.feature.course

import hu.bme.aut.android.homeworkmanagerapp.domain.model.course.CourseListRow
import hu.bme.aut.android.homeworkmanagerapp.network.course.CourseNetworkManager
import hu.bme.aut.android.homeworkmanagerapp.network.handleRequest
import retrofit2.HttpException
import javax.inject.Inject

class CourseHandler @Inject constructor(
    private val courseNetworkManager: CourseNetworkManager
) {
    suspend fun getCourses(onSuccess: (List<CourseListRow>) -> Unit, onError: () -> Unit) {
        try {
            val courses = handleRequest {
                courseNetworkManager.getCourses()
            }

            onSuccess(courses)
        } catch (httpException: HttpException) {
            onError()
        }
    }
}