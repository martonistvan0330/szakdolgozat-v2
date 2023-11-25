package hu.bme.aut.android.homeworkmanagerapp.feature.assignment.list

import android.content.Context
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import dagger.hilt.android.lifecycle.HiltViewModel
import dagger.hilt.android.qualifiers.ApplicationContext
import hu.bme.aut.android.homeworkmanagerapp.R
import hu.bme.aut.android.homeworkmanagerapp.feature.assignment.AssignmentHandler
import hu.bme.aut.android.homeworkmanagerapp.ui.model.assignment.AssignmentListRowUi
import hu.bme.aut.android.homeworkmanagerapp.ui.model.assignment.asAssignmentListRowUi
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import javax.inject.Inject

sealed class AssignmentListState {
    object Loading : AssignmentListState()
    data class Error(val error: Throwable) : AssignmentListState()
    data class Result(val assignmentList: List<AssignmentListRowUi>) : AssignmentListState()
}

@HiltViewModel
class AssignmentListViewModel @Inject constructor(
    private val assignmentHandler: AssignmentHandler,
    @ApplicationContext private val context: Context
) : ViewModel() {
    private val _state = MutableStateFlow<AssignmentListState>(AssignmentListState.Loading)
    val state = _state.asStateFlow()

    fun loadAssignments(groupId: Int?) {
        viewModelScope.launch {
            if (groupId == null) {
                assignmentHandler.getAssignments(
                    onSuccess = { result ->
                        _state.value = AssignmentListState.Result(
                            assignmentList = result.map { it.asAssignmentListRowUi() }
                        )
                    },
                    onError = {
                        _state.value =
                            AssignmentListState.Error(Exception(context.getString(R.string.something_went_wrong)))
                    }
                )
            }
        }
    }
}