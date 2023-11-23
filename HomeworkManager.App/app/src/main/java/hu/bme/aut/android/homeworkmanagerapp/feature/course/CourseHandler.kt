package hu.bme.aut.android.homeworkmanagerapp.feature.course

import hu.bme.aut.android.homeworkmanagerapp.domain.model.course.CourseListRow
import hu.bme.aut.android.homeworkmanagerapp.network.course.CourseNetworkManager
import hu.bme.aut.android.homeworkmanagerapp.network.handleAuthorize
import javax.inject.Inject

class CourseHandler @Inject constructor(
    private val courseNetworkManager: CourseNetworkManager
) {
    fun getCourses(onSuccess: (Array<CourseListRow>) -> Unit, onError: () -> Unit) {
        courseNetworkManager.getCourses().handleAuthorize(
            { response -> onSuccess(response) },
            { onError() },
        )
    }
}