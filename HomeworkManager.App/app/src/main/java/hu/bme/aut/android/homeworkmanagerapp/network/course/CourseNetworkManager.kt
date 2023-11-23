package hu.bme.aut.android.homeworkmanagerapp.network.course

import hu.bme.aut.android.homeworkmanagerapp.domain.model.course.CourseListRow
import retrofit2.Call
import javax.inject.Inject

class CourseNetworkManager @Inject constructor(
    private val courseApi: CourseApi
) {
    fun getCourses(): Call<Array<CourseListRow>>? {
        return courseApi.getCourses()
    }
}