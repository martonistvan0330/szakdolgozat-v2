package hu.bme.aut.android.homeworkmanagerapp.feature.course.list

import android.content.Context
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import dagger.hilt.android.lifecycle.HiltViewModel
import dagger.hilt.android.qualifiers.ApplicationContext
import hu.bme.aut.android.homeworkmanagerapp.R
import hu.bme.aut.android.homeworkmanagerapp.feature.course.CourseHandler
import hu.bme.aut.android.homeworkmanagerapp.ui.model.course.CourseListRowUi
import hu.bme.aut.android.homeworkmanagerapp.ui.model.course.asCourseListRowUi
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import javax.inject.Inject

sealed class CourseListState {
    object Loading : CourseListState()
    data class Error(val error: Throwable) : CourseListState()
    data class Result(val courseList: List<CourseListRowUi>) : CourseListState()
}

@HiltViewModel
class CourseListViewModel @Inject constructor(
    private val courseHandler: CourseHandler,
    @ApplicationContext private val context: Context
) : ViewModel() {
    private val _state = MutableStateFlow<CourseListState>(CourseListState.Loading)
    val state = _state.asStateFlow()

    init {
        loadCourses()
    }

    private fun loadCourses() {
        viewModelScope.launch {
            courseHandler.getCourses(
                onSuccess = { result ->
                    _state.value = CourseListState.Result(
                        courseList = result.map { it.asCourseListRowUi() }
                    )
                },
                onError = {
                    _state.value =
                        CourseListState.Error(Exception(context.getString(R.string.something_went_wrong)))
                }
            )
        }
    }
}
