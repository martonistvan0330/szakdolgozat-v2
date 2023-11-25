package hu.bme.aut.android.homeworkmanagerapp.feature.course.list

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
import androidx.compose.material.icons.outlined.School
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
import hu.bme.aut.android.homeworkmanagerapp.R
import hu.bme.aut.android.homeworkmanagerapp.ui.common.topbar.TopBar

@OptIn(ExperimentalMaterial3Api::class, ExperimentalFoundationApi::class)
@Composable
fun CourseListScreen(
    onLogout: () -> Unit,
    onListItemClick: (Int) -> Unit,
    viewModel: CourseListViewModel = hiltViewModel()
) {
    val state = viewModel.state.collectAsStateWithLifecycle().value

    Scaffold(
        topBar = {
            TopBar(
                title = stringResource(id = R.string.text_your_course_list),
                onLogout = onLogout
            )
        },
        bottomBar = {},
        snackbarHost = {},
        modifier = Modifier.fillMaxSize()
    ) {
        Box(
            modifier = Modifier
                .fillMaxSize()
                .padding(it)
                .background(
                    color = if (state is CourseListState.Loading || state is CourseListState.Error) {
                        MaterialTheme.colorScheme.secondaryContainer
                    } else {
                        MaterialTheme.colorScheme.background
                    }
                ),
            contentAlignment = Alignment.Center,
        ) {
            when (state) {
                is CourseListState.Loading -> CircularProgressIndicator(
                    color = MaterialTheme.colorScheme.onSecondaryContainer
                )

                is CourseListState.Error -> Text(
                    text = state.error.toString()
                )

                is CourseListState.Result -> {
                    if (state.courseList.isEmpty()) {
                        Text(text = stringResource(id = R.string.text_empty_course_list))
                    } else {
                        LazyColumn(
                            modifier = Modifier
                                .fillMaxSize()
                        ) {
                            items(
                                state.courseList,
                                key = { course -> course.courseId }) { course ->
                                ListItem(
                                    headlineText = {
                                        Row(verticalAlignment = Alignment.CenterVertically) {
                                            Icon(
                                                imageVector = Icons.Outlined.School,
                                                contentDescription = null,
                                                modifier = Modifier
                                                    .size(40.dp)
                                                    .padding(
                                                        end = 8.dp,
                                                        top = 8.dp,
                                                        bottom = 8.dp,
                                                    ),
                                            )
                                            Text(text = course.name)
                                        }
                                    },
                                    supportingText = {
                                        Text(
                                            text = course.name,
                                        )
                                    },
                                    modifier = Modifier
                                        .clickable(onClick = {
                                            onListItemClick(
                                                course.courseId,
                                            )
                                        })
                                        .animateItemPlacement(),
                                )
                                if (state.courseList.last() != course) {
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