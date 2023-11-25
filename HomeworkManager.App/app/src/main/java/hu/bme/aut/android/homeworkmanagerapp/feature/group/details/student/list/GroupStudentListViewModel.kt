package hu.bme.aut.android.homeworkmanagerapp.feature.group.details.student.list

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import androidx.paging.PagingData
import androidx.paging.cachedIn
import dagger.hilt.android.lifecycle.HiltViewModel
import hu.bme.aut.android.homeworkmanagerapp.feature.group.GroupHandler
import hu.bme.aut.android.homeworkmanagerapp.ui.model.user.UserListRowUi
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import javax.inject.Inject

sealed class GroupStudentListState {
    object Loading : GroupStudentListState()
    data class Result(
        val studentList: Flow<PagingData<UserListRowUi>>
    ) : GroupStudentListState()
}

@HiltViewModel
class GroupStudentListViewModel @Inject constructor(
    private val groupHandler: GroupHandler
) : ViewModel() {
    private val _searchTextState = MutableStateFlow("")
    val searchTextState = _searchTextState.asStateFlow()

    private val _state = MutableStateFlow<GroupStudentListState>(GroupStudentListState.Loading)
    val state = _state.asStateFlow()

    fun updateSearchText(newValue: String) {
        _searchTextState.value = newValue
    }

    fun loadStudents(courseId: Int, groupName: String) {
        val students = groupHandler
            .getStudents(courseId, groupName, _searchTextState.value)
            .cachedIn(viewModelScope)
        _state.value = GroupStudentListState.Result(studentList = students)
    }
}