package hu.bme.aut.android.homeworkmanagerapp.feature.assignment.list

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import androidx.paging.PagingData
import androidx.paging.cachedIn
import dagger.hilt.android.lifecycle.HiltViewModel
import hu.bme.aut.android.homeworkmanagerapp.feature.assignment.AssignmentHandler
import hu.bme.aut.android.homeworkmanagerapp.ui.model.assignment.AssignmentListRowUi
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import javax.inject.Inject

sealed class AssignmentListState {
    object Loading : AssignmentListState()
    data class Result(
        val assignmentList: Flow<PagingData<AssignmentListRowUi>>
    ) : AssignmentListState()
}

@HiltViewModel
class AssignmentListViewModel @Inject constructor(
    private val assignmentHandler: AssignmentHandler
) : ViewModel() {
    private val _searchTextState = MutableStateFlow("")
    val searchTextState = _searchTextState.asStateFlow()

    private val _state = MutableStateFlow<AssignmentListState>(AssignmentListState.Loading)
    val state = _state.asStateFlow()

    fun updateSearchText(newValue: String) {
        _searchTextState.value = newValue
    }

    fun loadAssignments(courseId: Int?, groupName: String?) {
        val assignments = if (courseId == null) {
            assignmentHandler
                .getAssignments(_searchTextState.value)
                .cachedIn(viewModelScope)
        } else {
            if (groupName == null) {
                assignmentHandler
                    .getAssignments(courseId, "General", _searchTextState.value)
                    .cachedIn(viewModelScope)
            } else {
                assignmentHandler
                    .getAssignments(courseId, groupName, _searchTextState.value)
                    .cachedIn(viewModelScope)
            }
        }
        _state.value = AssignmentListState.Result(assignmentList = assignments)
    }
}