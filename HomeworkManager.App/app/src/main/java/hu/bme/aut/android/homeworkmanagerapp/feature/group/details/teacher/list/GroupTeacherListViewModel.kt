package hu.bme.aut.android.homeworkmanagerapp.feature.group.details.teacher.list

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

sealed class GroupTeacherListState {
    object Loading : GroupTeacherListState()
    data class Result(
        val teacherList: Flow<PagingData<UserListRowUi>>
    ) : GroupTeacherListState()
}

@HiltViewModel
class GroupTeacherListViewModel @Inject constructor(
    private val groupHandler: GroupHandler
) : ViewModel() {
    private val _searchTextState = MutableStateFlow("")
    val searchTextState = _searchTextState.asStateFlow()

    private val _state = MutableStateFlow<GroupTeacherListState>(GroupTeacherListState.Loading)
    val state = _state.asStateFlow()

    fun updateSearchText(newValue: String) {
        _searchTextState.value = newValue
    }

    fun loadTeachers(courseId: Int, groupName: String) {
        val teachers = groupHandler
            .getTeachers(courseId, groupName, _searchTextState.value)
            .cachedIn(viewModelScope)
        _state.value = GroupTeacherListState.Result(teacherList = teachers)
    }
}