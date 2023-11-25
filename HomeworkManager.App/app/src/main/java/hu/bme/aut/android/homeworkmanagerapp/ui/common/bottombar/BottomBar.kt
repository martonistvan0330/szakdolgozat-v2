package hu.bme.aut.android.homeworkmanagerapp.ui.common.bottombar

import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Assignment
import androidx.compose.material.icons.filled.School
import androidx.compose.material3.BottomAppBar
import androidx.compose.material3.Icon
import androidx.compose.material3.NavigationBarItem
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.navigation.NavController
import androidx.navigation.NavDestination.Companion.hierarchy
import androidx.navigation.compose.currentBackStackEntryAsState
import hu.bme.aut.android.homeworkmanagerapp.navigation.Screen


@Composable
fun BottomBar(
    navController: NavController
) {
    BottomAppBar {
        val navBackStackEntry by navController.currentBackStackEntryAsState()
        val currentDestination = navBackStackEntry?.destination
        NavigationBarItem(
            icon = { Icon(Icons.Filled.School, contentDescription = null) },
            label = { Text(text = "Courses") },
            selected = currentDestination?.hierarchy?.any { it.route == Screen.CourseList.routePattern } == true,
            onClick = {
                navController.navigate(Screen.CourseList.routePattern) {
                    popUpTo(navController.graph.id) {
                        saveState = true
                    }
                    launchSingleTop = true
                    restoreState = true
                }
            },
        )

        NavigationBarItem(
            icon = { Icon(Icons.Filled.Assignment, contentDescription = null) },
            label = { Text(text = "Assignments") },
            selected = currentDestination?.hierarchy?.any { it.route == Screen.AssignmentList.routePattern } == true,
            onClick = {
                navController.navigate(Screen.AssignmentList.routePattern) {
                    popUpTo(navController.graph.id) {
                        saveState = true
                    }
                    launchSingleTop = true
                    restoreState = true
                }
            },
        )
    }
}