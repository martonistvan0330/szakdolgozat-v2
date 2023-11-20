package hu.bme.aut.android.homeworkmanagerapp.feature.course

import android.content.Context
import hu.bme.aut.android.homeworkmanagerapp.domain.model.course.CourseListRow
import hu.bme.aut.android.homeworkmanagerapp.network.course.CourseNetworkManager
import hu.bme.aut.android.homeworkmanagerapp.network.handleAuthorize

class CourseHandler(private val context: Context) {
    fun getCourses(onSuccess: (Array<CourseListRow>) -> Unit, onError: () -> Unit) {
        CourseNetworkManager(context).getCourses().handleAuthorize(
            { response -> onSuccess(response) },
            { onError() },
        )
    }
}