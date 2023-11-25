package hu.bme.aut.android.homeworkmanagerapp.feature.group.details

import androidx.compose.foundation.ExperimentalFoundationApi
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.pager.HorizontalPager
import androidx.compose.foundation.pager.rememberPagerState
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.sp
import androidx.hilt.navigation.compose.hiltViewModel
import androidx.navigation.NavHostController
import hu.bme.aut.android.homeworkmanagerapp.feature.assignment.list.AssignmentListScreen
import hu.bme.aut.android.homeworkmanagerapp.feature.group.details.student.list.GroupStudentListScreen
import hu.bme.aut.android.homeworkmanagerapp.feature.group.details.teacher.list.GroupTeacherListScreen
import hu.bme.aut.android.homeworkmanagerapp.ui.common.bottombar.BottomBar
import hu.bme.aut.android.homeworkmanagerapp.ui.common.topbar.TopBar

@OptIn(ExperimentalMaterial3Api::class, ExperimentalFoundationApi::class)
@Composable
fun GroupDetailsScreen(
    courseId: Int,
    groupName: String,
    onLogout: () -> Unit,
    navController: NavHostController,
    viewModel: GroupDetailsViewModel = hiltViewModel()
) {
    Scaffold(
        topBar = {
            TopBar(
                title = groupName,
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
        Box(
            modifier = Modifier
                .fillMaxSize()
                .padding(padding),
            contentAlignment = Alignment.Center,
        ) {
            val pagerState = rememberPagerState(pageCount = { 3 })

            HorizontalPager(state = pagerState) { page ->
                when (page) {
                    0 -> {
                        Column {
                            Text(
                                text = "Assignments",
                                modifier = Modifier
                                    .fillMaxWidth(),
                                textAlign = TextAlign.Center,
                                fontSize = 24.sp
                            )
                            AssignmentListScreen(
                                courseId = courseId,
                                groupName = groupName,
                                onListItemClick = { },
                            )
                        }
                    }

                    1 -> {
                        Column {
                            Text(
                                text = "Teachers",
                                modifier = Modifier
                                    .fillMaxWidth(),
                                textAlign = TextAlign.Center,
                                fontSize = 24.sp
                            )
                            GroupTeacherListScreen(
                                courseId = courseId,
                                groupName = groupName,
                                onListItemClick = { },
                            )
                        }
                    }

                    2 -> {
                        Column {
                            Text(
                                text = "Students",
                                modifier = Modifier
                                    .fillMaxWidth(),
                                textAlign = TextAlign.Center,
                                fontSize = 24.sp
                            )
                            GroupStudentListScreen(
                                courseId = courseId,
                                groupName = groupName,
                                onListItemClick = { },
                            )
                        }
                    }

                    else -> {
                        Column {
                            Text(
                                text = "Assignments",
                                modifier = Modifier
                                    .fillMaxWidth(),
                                textAlign = TextAlign.Center
                            )
                            AssignmentListScreen(
                                courseId = courseId,
                                groupName = groupName,
                                onListItemClick = { },
                            )
                        }
                    }
                }
            }
        }
    }
}