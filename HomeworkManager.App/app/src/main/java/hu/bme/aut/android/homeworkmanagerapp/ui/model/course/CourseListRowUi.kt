package hu.bme.aut.android.homeworkmanagerapp.ui.model.course

import hu.bme.aut.android.homeworkmanagerapp.domain.model.course.CourseListRow

data class CourseListRowUi(
    val courseId: Int = 0,
    val name: String = "",
)

fun CourseListRow.asCourseListRowUi(): CourseListRowUi = CourseListRowUi(
    courseId = courseId,
    name = name,
)

fun CourseListRowUi.asCourseListRow(): CourseListRow = CourseListRow(
    courseId = courseId,
    name = name,
)