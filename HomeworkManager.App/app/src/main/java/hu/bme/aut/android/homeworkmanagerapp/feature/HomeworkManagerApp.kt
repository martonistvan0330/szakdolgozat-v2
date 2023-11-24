package hu.bme.aut.android.homeworkmanagerapp.feature

import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.rememberCoroutineScope
import androidx.hilt.navigation.compose.hiltViewModel
import androidx.navigation.compose.rememberNavController
import hu.bme.aut.android.homeworkmanagerapp.navigation.NavGraph

@Composable
fun HomeworkManagerApp(
    viewModel: HomeworkManagerAppViewModel = hiltViewModel()
) {
    val scope = rememberCoroutineScope()

    val loggedIn by viewModel.loggedIn.collectAsState()

    val navController = rememberNavController()
    NavGraph(
        navController = navController,
        loggedIn = loggedIn
    )
}