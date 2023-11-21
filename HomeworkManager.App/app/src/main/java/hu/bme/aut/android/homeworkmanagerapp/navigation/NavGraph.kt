package hu.bme.aut.android.homeworkmanagerapp.navigation

import androidx.compose.runtime.Composable
import androidx.navigation.NavGraphBuilder
import androidx.navigation.NavHostController
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.navigation
import hu.bme.aut.android.homeworkmanagerapp.feature.auth.login.LoginScreen
import hu.bme.aut.android.homeworkmanagerapp.feature.auth.register.RegisterScreen
import hu.bme.aut.android.homeworkmanagerapp.feature.course.list.CourseListScreen

@Composable
fun NavGraph(
    navController: NavHostController,
) {
    NavHost(
        navController = navController,
        startDestination = AUTH_GRAPH_ROUTE,
        route = ROOT_GRAPH_ROUTE,
    ) {
        authNavGraph(navController = navController)
        courseNavGraph(navController = navController)
        groupNavGraph(navController = navController)
    }
}


fun NavGraphBuilder.authNavGraph(
    navController: NavHostController,
) {
    navigation(
        startDestination = Screen.Login.routePattern,
        route = AUTH_GRAPH_ROUTE,
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
                    navController.navigate(Screen.Login.navigationRoute) {
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
}

fun NavGraphBuilder.courseNavGraph(
    navController: NavHostController,
) {
    navigation(
        startDestination = Screen.CourseList.routePattern,
        route = COURSE_GRAPH_ROUTE,
    ) {
        composable(
            route = Screen.CourseList.routePattern,
        ) {
            CourseListScreen(
                onListItemClick = { courseId ->
                    navController.navigate(Screen.GroupList.navigationRoute + courseId)
                },
            )
        }
    }
}
