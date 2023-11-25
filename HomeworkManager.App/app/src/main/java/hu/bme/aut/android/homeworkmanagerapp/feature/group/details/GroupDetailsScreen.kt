package hu.bme.aut.android.homeworkmanagerapp.feature.group.details

import androidx.compose.foundation.ExperimentalFoundationApi
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.pager.HorizontalPager
import androidx.compose.foundation.pager.rememberPagerState
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.Scaffold
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.hilt.navigation.compose.hiltViewModel
import androidx.navigation.NavHostController
import hu.bme.aut.android.homeworkmanagerapp.feature.assignment.list.AssignmentListScreen
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
                        AssignmentListScreen(
                            courseId = courseId,
                            groupName = groupName,
                            onListItemClick = { },
                        )
                    }

                    else -> {
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