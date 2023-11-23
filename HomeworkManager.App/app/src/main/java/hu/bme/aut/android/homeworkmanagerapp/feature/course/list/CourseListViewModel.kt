package hu.bme.aut.android.homeworkmanagerapp.feature.course.list

import androidx.lifecycle.ViewModel
import dagger.hilt.android.lifecycle.HiltViewModel
import hu.bme.aut.android.homeworkmanagerapp.feature.course.CourseHandler
import hu.bme.aut.android.homeworkmanagerapp.ui.model.course.CourseListRowUi
import hu.bme.aut.android.homeworkmanagerapp.ui.model.course.asCourseListRowUi
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import javax.inject.Inject

sealed class CourseListState {
    object Loading : CourseListState()
    data class Error(val error: Throwable) : CourseListState()
    data class Result(val courseList: List<CourseListRowUi>) : CourseListState()
}

@HiltViewModel
class CourseListViewModel @Inject constructor(
    private val courseHandler: CourseHandler
) : ViewModel() {
    private val _state = MutableStateFlow<CourseListState>(CourseListState.Loading)
    val state = _state.asStateFlow()

    init {
        loadCourses()
    }

    private fun loadCourses() {
        courseHandler.getCourses(
            { result ->
                _state.value = CourseListState.Result(
                    courseList = result.map { it.asCourseListRowUi() },
                )
            },
            { _state.value = CourseListState.Error(Exception("ERROR")) },
        )
    }
}
