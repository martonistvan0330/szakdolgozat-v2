package hu.bme.aut.android.homeworkmanagerapp.feature.assignment.list

import androidx.compose.foundation.ExperimentalFoundationApi
import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Search
import androidx.compose.material.icons.outlined.Assignment
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.Divider
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.ListItem
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import androidx.hilt.navigation.compose.hiltViewModel
import androidx.navigation.NavHostController
import androidx.paging.compose.collectAsLazyPagingItems
import hu.bme.aut.android.homeworkmanagerapp.R
import hu.bme.aut.android.homeworkmanagerapp.ui.common.NormalTextField
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
    ) { padding ->
        val searchText by viewModel.searchTextState.collectAsState()
        val state = viewModel.state.collectAsState().value

        Box(
            modifier = Modifier
                .fillMaxSize()
                .padding(padding),
            contentAlignment = Alignment.Center,
        ) {
            Column {
                NormalTextField(
                    modifier = Modifier
                        .fillMaxWidth(),
                    value = searchText,
                    label = "Search",
                    onValueChange = { newValue ->
                        viewModel.updateSearchText(newValue)
                    },
                    leadingIcon = { },
                    trailingIcon = {
                        IconButton(onClick = {
                            viewModel.loadAssignments(groupId)
                        }) {
                            Icon(Icons.Default.Search, null)
                        }
                    },
                    onDone = { viewModel.loadAssignments(groupId) }
                )
                Box(
                    modifier = Modifier
                        .fillMaxSize()
                        .background(
                            color = if (state is AssignmentListState.Loading) {
                                MaterialTheme.colorScheme.secondaryContainer
                            } else {
                                MaterialTheme.colorScheme.background
                            }
                        ),
                ) {
                    when (state) {
                        is AssignmentListState.Loading -> CircularProgressIndicator(
                            color = MaterialTheme.colorScheme.onSecondaryContainer
                        )

                        is AssignmentListState.Result -> {
                            val assignments = state.assignmentList.collectAsLazyPagingItems()

                            LazyColumn(
                                modifier = Modifier
                                    .fillMaxSize()
                            ) {
                                items(
                                    count = assignments.itemCount,
                                    key = { index ->
                                        val assignment = assignments[index]
                                        "${assignment?.assignmentId ?: ""}$index"
                                    }
                                ) { index ->
                                    val assignment = assignments[index] ?: return@items
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
                                    if (assignments.itemCount != index) {
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
}