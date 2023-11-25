package hu.bme.aut.android.homeworkmanagerapp.feature.group.details.student.list

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
import androidx.compose.material.icons.outlined.Person
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
import androidx.compose.ui.unit.dp
import androidx.hilt.navigation.compose.hiltViewModel
import androidx.paging.compose.collectAsLazyPagingItems
import hu.bme.aut.android.homeworkmanagerapp.ui.common.NormalTextField

@OptIn(ExperimentalMaterial3Api::class, ExperimentalFoundationApi::class)
@Composable
fun GroupStudentListScreen(
    courseId: Int,
    groupName: String,
    onListItemClick: (String) -> Unit,
    viewModel: GroupStudentListViewModel = hiltViewModel()
) {
    viewModel.loadStudents(courseId, groupName)

    Scaffold(
        topBar = {},
        bottomBar = {},
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
                            viewModel.loadStudents(courseId, groupName)
                        }) {
                            Icon(Icons.Default.Search, null)
                        }
                    },
                    onDone = { viewModel.loadStudents(courseId, groupName) }
                )
                Box(
                    modifier = Modifier
                        .fillMaxSize()
                        .background(
                            color = if (state is GroupStudentListState.Loading) {
                                MaterialTheme.colorScheme.secondaryContainer
                            } else {
                                MaterialTheme.colorScheme.background
                            }
                        ),
                ) {
                    when (state) {
                        is GroupStudentListState.Loading -> CircularProgressIndicator(
                            color = MaterialTheme.colorScheme.onSecondaryContainer
                        )

                        is GroupStudentListState.Result -> {
                            val students = state.studentList.collectAsLazyPagingItems()

                            LazyColumn(
                                modifier = Modifier
                                    .fillMaxSize()
                            ) {
                                items(
                                    count = students.itemCount,
                                    key = { index ->
                                        val student = students[index]
                                        "${student?.userId ?: ""}$index"
                                    }
                                ) { index ->
                                    val student = students[index] ?: return@items
                                    ListItem(
                                        headlineText = {
                                            Row(verticalAlignment = Alignment.CenterVertically) {
                                                Icon(
                                                    imageVector = Icons.Outlined.Person,
                                                    contentDescription = null,
                                                    modifier = Modifier
                                                        .size(40.dp)
                                                        .padding(
                                                            end = 8.dp,
                                                            top = 8.dp,
                                                            bottom = 8.dp,
                                                        ),
                                                )
                                                Text(text = student.fullName)
                                            }
                                        },
                                        supportingText = {
                                            Text(
                                                text = student.email,
                                            )
                                        },
                                        modifier = Modifier
                                            .clickable(onClick = {
                                                onListItemClick(
                                                    student.userId,
                                                )
                                            })
                                            .animateItemPlacement(),
                                    )
                                    if (students.itemCount != index) {
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