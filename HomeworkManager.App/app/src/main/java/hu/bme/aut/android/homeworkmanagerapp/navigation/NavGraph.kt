package hu.bme.aut.android.homeworkmanagerapp.navigation

import androidx.compose.runtime.Composable
import androidx.navigation.NavGraphBuilder
import androidx.navigation.NavHostController
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import hu.bme.aut.android.homeworkmanagerapp.feature.auth.login.LoginScreen
import hu.bme.aut.android.homeworkmanagerapp.feature.auth.register.RegisterScreen
import hu.bme.aut.android.homeworkmanagerapp.feature.course.list.CourseListScreen

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
        //groupNavGraph(navController = navController)
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
                    popUpTo(Screen.Login.routePattern) {
                        inclusive = true
                    }
                }
            },
            onRegisterClick = {
                navController.navigate(Screen.Register.navigationRoute) {
                    popUpTo(Screen.Login.routePattern) {
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
                    popUpTo(Screen.Register.routePattern) {
                        inclusive = true
                    }
                }
            },
            onLoginClick = {
                navController.navigate(Screen.Login.navigationRoute) {
                    popUpTo(Screen.Register.routePattern) {
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
            onListItemClick = { courseId ->
                navController.navigate(Screen.CourseList.navigationRoute + courseId + Screen.GroupList.navigationRoute)
            },
        )
    }
}
