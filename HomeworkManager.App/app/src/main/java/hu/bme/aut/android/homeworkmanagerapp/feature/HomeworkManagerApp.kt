package hu.bme.aut.android.homeworkmanagerapp.feature

import androidx.compose.runtime.Composable
import androidx.navigation.compose.rememberNavController
import hu.bme.aut.android.homeworkmanagerapp.navigation.NavGraph

@Composable
fun HomeworkManagerApp() {
    val navController = rememberNavController()
    NavGraph(navController = navController)
}