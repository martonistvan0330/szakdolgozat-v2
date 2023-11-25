package hu.bme.aut.android.homeworkmanagerapp.feature.group.list

import android.content.Context
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import dagger.hilt.android.lifecycle.HiltViewModel
import dagger.hilt.android.qualifiers.ApplicationContext
import hu.bme.aut.android.homeworkmanagerapp.R
import hu.bme.aut.android.homeworkmanagerapp.feature.group.GroupHandler
import hu.bme.aut.android.homeworkmanagerapp.ui.model.group.GroupListRowUi
import hu.bme.aut.android.homeworkmanagerapp.ui.model.group.asGroupListRowUi
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch
import javax.inject.Inject

sealed class GroupListState {
    object Loading : GroupListState()
    data class Error(val error: Throwable) : GroupListState()
    data class Result(val groupList: List<GroupListRowUi>) : GroupListState()
}

@HiltViewModel
class GroupListViewModel @Inject constructor(
    private val groupHandler: GroupHandler,
    @ApplicationContext private val context: Context
) : ViewModel() {
    private val _state = MutableStateFlow<GroupListState>(GroupListState.Loading)
    val state = _state.asStateFlow()

    fun loadGroups(courseId: Int) {
        viewModelScope.launch {
            groupHandler.getGroups(
                courseId,
                onSuccess = { result ->
                    _state.value = GroupListState.Result(
                        groupList = result.map { it.asGroupListRowUi() }
                    )
                },
                onError = {
                    _state.value =
                        GroupListState.Error(Exception(context.getString(R.string.something_went_wrong)))
                }
            )
        }
    }
}