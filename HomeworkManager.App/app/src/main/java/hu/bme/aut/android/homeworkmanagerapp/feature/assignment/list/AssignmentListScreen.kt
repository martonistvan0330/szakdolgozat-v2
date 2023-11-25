package hu.bme.aut.android.homeworkmanagerapp.feature.assignment.list

import androidx.compose.foundation.ExperimentalFoundationApi
import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.outlined.Assignment
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.Divider
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.Icon
import androidx.compose.material3.ListItem
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import androidx.hilt.navigation.compose.hiltViewModel
import androidx.lifecycle.compose.collectAsStateWithLifecycle
import androidx.navigation.NavHostController
import hu.bme.aut.android.homeworkmanagerapp.R
import hu.bme.aut.android.homeworkmanagerapp.ui.common.bottombar.BottomBar
import hu.bme.aut.android.homeworkmanagerapp.ui.common.topbar.TopBar

@OptIn(ExperimentalMaterial3Api::class, ExperimentalFoundationApi::class)
@Composable
fun AssignmentListScreen(
    groupId: Int? = null,
    onLogout: () -> Unit,
    onListItemClick: (Int) -> Unit,
    navController: NavHostController,
    viewModel: AssignmentListViewModel = hiltViewModel()
) {
    val state = viewModel.state.collectAsStateWithLifecycle().value
    viewModel.loadAssignments(groupId)

    Scaffold(
        topBar = {
            TopBar(
                title = stringResource(id = R.string.text_your_assignment_list),
                onLogout = onLogout
            )
        },
        bottomBar = {
            BottomBar(
                navController = navController
            )
        },
        snackbarHost = {},
        modifier = Modifier.fillMaxSize()
    ) {
        Box(
            modifier = Modifier
                .fillMaxSize()
                .padding(it)
                .background(
                    color = if (state is AssignmentListState.Loading || state is AssignmentListState.Error) {
                        MaterialTheme.colorScheme.secondaryContainer
                    } else {
                        MaterialTheme.colorScheme.background
                    }
                ),
            contentAlignment = Alignment.Center,
        ) {
            when (state) {
                is AssignmentListState.Loading -> CircularProgressIndicator(
                    color = MaterialTheme.colorScheme.onSecondaryContainer
                )

                is AssignmentListState.Error -> Text(
                    text = state.error.toString()
                )

                is AssignmentListState.Result -> {
                    if (state.assignmentList.isEmpty()) {
                        Text(text = stringResource(id = R.string.text_empty_assignment_list))
                    } else {
                        LazyColumn(
                            modifier = Modifier
                                .fillMaxSize()
                        ) {
                            items(
                                state.assignmentList,
                                key = { assignment -> assignment.assignmentId }) { assignment ->
                                ListItem(
                                    headlineText = {
                                        Row(verticalAlignment = Alignment.CenterVertically) {
                                            Icon(
                                                imageVector = Icons.Outlined.Assignment,
                                                contentDescription = null,
                                                modifier = Modifier
                                                    .size(40.dp)
                                                    .padding(
                                                        end = 8.dp,
                                                        top = 8.dp,
                                                        bottom = 8.dp,
                                                    ),
                                            )
                                            Text(text = assignment.name)
                                        }
                                    },
                                    supportingText = {
                                        Text(
                                            text = assignment.deadline.toString(),
                                        )
                                    },
                                    modifier = Modifier
                                        .clickable(onClick = {
                                            onListItemClick(
                                                assignment.assignmentId,
                                            )
                                        })
                                        .animateItemPlacement(),
                                )
                                if (state.assignmentList.last() != assignment) {
                                    Divider(
                                        thickness = 2.dp,
                                        color = MaterialTheme.colorScheme.secondaryContainer,
                                    )
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}