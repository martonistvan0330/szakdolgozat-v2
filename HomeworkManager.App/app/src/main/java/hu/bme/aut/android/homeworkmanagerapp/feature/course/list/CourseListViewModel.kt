package hu.bme.aut.android.homeworkmanagerapp.feature.course.list

import android.content.Context
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.viewmodel.initializer
import androidx.lifecycle.viewmodel.viewModelFactory
import hu.bme.aut.android.homeworkmanagerapp.feature.course.CourseHandler
import hu.bme.aut.android.homeworkmanagerapp.ui.model.course.CourseListRowUi
import hu.bme.aut.android.homeworkmanagerapp.ui.model.course.asCourseListRowUi
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow

sealed class CourseListState {
    object Loading : CourseListState()
    data class Error(val error: Throwable) : CourseListState()
    data class Result(val courseList: List<CourseListRowUi>) : CourseListState()
}

class CourseListViewModel(private val courseHandler: CourseHandler) : ViewModel() {
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

    companion object {
        fun factory(context: Context): ViewModelProvider.Factory = viewModelFactory {
            initializer {
                CourseListViewModel(CourseHandler(context))
            }
        }
    }
}
