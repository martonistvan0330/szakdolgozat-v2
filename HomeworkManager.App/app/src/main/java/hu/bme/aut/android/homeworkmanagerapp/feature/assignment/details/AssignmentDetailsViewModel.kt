package hu.bme.aut.android.homeworkmanagerapp.feature.assignment.details

import android.content.Context
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import dagger.hilt.android.lifecycle.HiltViewModel
import dagger.hilt.android.qualifiers.ApplicationContext
import hu.bme.aut.android.homeworkmanagerapp.R
import hu.bme.aut.android.homeworkmanagerapp.feature.assignment.AssignmentHandler
import hu.bme.aut.android.homeworkmanagerapp.ui.model.assignment.AssignmentModelUi
import hu.bme.aut.android.homeworkmanagerapp.ui.model.assignment.asAssignmentListRowUi
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import javax.inject.Inject

sealed class AssignmentDetailsState {
    object Loading : AssignmentDetailsState()
    data class Error(val error: Throwable) : AssignmentDetailsState()
    data class Result(val assignment: AssignmentModelUi) : AssignmentDetailsState()
}

@HiltViewModel
class AssignmentDetailsViewModel @Inject constructor(
    private val assignmentHandler: AssignmentHandler,
    @ApplicationContext private val context: Context
) : ViewModel() {
    private val _state = MutableStateFlow<AssignmentDetailsState>(AssignmentDetailsState.Loading)
    val state = _state.asStateFlow()

    fun loadAssignment(assignmentId: Int) {
        viewModelScope.launch {
            assignmentHandler.getAssignment(
                assignmentId = assignmentId,
                onSuccess = { result ->
                    _state.value = AssignmentDetailsState.Result(
                        assignment = result.asAssignmentListRowUi()
                    )
                },
                onError = {
                    AssignmentDetailsState.Error(
                        Exception(context.getString(R.string.something_went_wrong))
                    )
                }
            )
        }
    }
}