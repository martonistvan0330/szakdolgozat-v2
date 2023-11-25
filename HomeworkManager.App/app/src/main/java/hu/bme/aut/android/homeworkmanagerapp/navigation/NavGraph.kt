package hu.bme.aut.android.homeworkmanagerapp.navigation

import androidx.compose.runtime.Composable
import androidx.navigation.NavGraphBuilder
import androidx.navigation.NavHostController
import androidx.navigation.NavType
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.navArgument
import hu.bme.aut.android.homeworkmanagerapp.feature.auth.login.LoginScreen
import hu.bme.aut.android.homeworkmanagerapp.feature.auth.register.RegisterScreen
import hu.bme.aut.android.homeworkmanagerapp.feature.course.list.CourseListScreen
import hu.bme.aut.android.homeworkmanagerapp.feature.group.list.GroupListScreen

@Composable
fun NavGraph(
    navController: NavHostController,
    loggedIn: Boolean = false
) {
    NavHost(
        navController = navController,
        startDestination = if (loggedIn) Screen.CourseList.routePattern else Screen.Login.routePattern,
    ) {
        authNavGraph(navController = navController)
        courseNavGraph(navController = navController)
        groupNavGraph(navController = navController)
    }
}


fun NavGraphBuilder.authNavGraph(
    navController: NavHostController,
) {
    composable(
        route = Screen.Login.routePattern,
    ) {
        LoginScreen(
            onLogin = {
                navController.navigate(Screen.CourseList.navigationRoute) {
                    popUpTo(navController.graph.id) {
                        inclusive = true
                    }
                }
            },
            onRegisterClick = {
                navController.navigate(Screen.Register.navigationRoute) {
                    popUpTo(navController.graph.id) {
                        inclusive = true
                    }
                }
            },
        )
    }
    composable(
        route = Screen.Register.routePattern
    ) {
        RegisterScreen(
            onRegister = {
                navController.navigate(Screen.CourseList.navigationRoute) {
                    popUpTo(navController.graph.id) {
                        inclusive = true
                    }
                }
            },
            onLoginClick = {
                navController.navigate(Screen.Login.navigationRoute) {
                    popUpTo(navController.graph.id) {
                        inclusive = true
                    }
                }
            },
        )
    }
}

fun NavGraphBuilder.courseNavGraph(
    navController: NavHostController,
) {
    composable(
        route = Screen.CourseList.routePattern,
    ) {
        CourseListScreen(
            onLogout = {
                navController.navigate(Screen.Login.navigationRoute) {
                    popUpTo(navController.graph.id) {
                        inclusive = true
                    }
                }
            },
            onListItemClick = { courseId ->
                navController.navigate("${Screen.CourseList.navigationRoute}/$courseId/${Screen.GroupList.navigationRoute}")
            },
        )
    }
}

fun NavGraphBuilder.groupNavGraph(
    navController: NavHostController,
) {
    composable(
        route = Screen.GroupList.routePattern,
        arguments = listOf(navArgument("courseId") { type = NavType.IntType })
    ) { backStackEntry ->
        GroupListScreen(
            backStackEntry.arguments!!.getInt("courseId"),
            onLogout = {
                navController.navigate(Screen.Login.navigationRoute) {
                    popUpTo(navController.graph.id) {
                        inclusive = true
                    }
                }
            },
            onListItemClick = { groupId ->
                // TODO
            },
        )
    }
}
