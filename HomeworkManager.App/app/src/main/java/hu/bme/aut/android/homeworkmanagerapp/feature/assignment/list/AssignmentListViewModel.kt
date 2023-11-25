package hu.bme.aut.android.homeworkmanagerapp.feature.assignment.list

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import androidx.paging.cachedIn
import dagger.hilt.android.lifecycle.HiltViewModel
import hu.bme.aut.android.homeworkmanagerapp.feature.assignment.AssignmentHandler
import hu.bme.aut.android.homeworkmanagerapp.ui.model.assignment.AssignmentListRowUi
import javax.inject.Inject

sealed class AssignmentListState {
    object Loading : AssignmentListState()
    data class Error(val error: Throwable) : AssignmentListState()
    data class Result(val assignmentList: List<AssignmentListRowUi>) : AssignmentListState()
}

@HiltViewModel
class AssignmentListViewModel @Inject constructor(
    assignmentHandler: AssignmentHandler,
    //@ApplicationContext private val context: Context
) : ViewModel() {

    val assignments = assignmentHandler.getAssignments().cachedIn(viewModelScope)

//    private val _state = MutableStateFlow<AssignmentListState>(AssignmentListState.Loading)
//    val state = _state.asStateFlow()
//
//    fun loadAssignments(groupId: Int?) {
//        viewModelScope.launch {
//            if (groupId == null) {
//                assignmentHandler.getAssignments(
//                    onSuccess = { result ->
//                        _state.value = AssignmentListState.Result(
//                            assignmentList = result.map { it.asAssignmentListRowUi() }
//                        )
//                    },
//                    onError = {
//                        _state.value =
//                            AssignmentListState.Error(Exception(context.getString(R.string.something_went_wrong)))
//                    }
//                )
//            }
//        }
//    }
}