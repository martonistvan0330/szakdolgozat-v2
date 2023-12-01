package hu.bme.aut.android.homeworkmanagerapp.navigation

import android.os.Build
import androidx.annotation.RequiresApi
import androidx.compose.runtime.Composable
import androidx.navigation.NavGraphBuilder
import androidx.navigation.NavHostController
import androidx.navigation.NavType
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.navArgument
import hu.bme.aut.android.homeworkmanagerapp.feature.appointment.grid.AppointmentGridScreen
import hu.bme.aut.android.homeworkmanagerapp.feature.assignment.details.AssignmentDetailsScreen
import hu.bme.aut.android.homeworkmanagerapp.feature.assignment.list.AssignmentListScreen
import hu.bme.aut.android.homeworkmanagerapp.feature.auth.login.LoginScreen
import hu.bme.aut.android.homeworkmanagerapp.feature.auth.register.RegisterScreen
import hu.bme.aut.android.homeworkmanagerapp.feature.course.list.CourseListScreen
import hu.bme.aut.android.homeworkmanagerapp.feature.group.details.GroupDetailsScreen
import hu.bme.aut.android.homeworkmanagerapp.feature.group.list.GroupListScreen

@RequiresApi(Build.VERSION_CODES.Q)
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
        assignmentNavGraph(navController = navController)
        appointmentNavGraph(navController = navController)
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
            navController = navController
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
        val courseId = backStackEntry.arguments!!.getInt("courseId")

        GroupListScreen(
            courseId,
            onLogout = {
                navController.navigate(Screen.Login.navigationRoute) {
                    popUpTo(navController.graph.id) {
                        inclusive = true
                    }
                }
            },
            onListItemClick = { groupName ->
                navController.navigate(
                    "${Screen.CourseList.navigationRoute}/$courseId/${Screen.GroupDetails.navigationRoute}/$groupName"
                )
            },
            navController = navController
        )
    }
    composable(
        route = Screen.GroupDetails.routePattern,
        arguments = listOf(
            navArgument("courseId") { type = NavType.IntType },
            navArgument("groupName") { type = NavType.StringType }
        )
    ) { backStackEntry ->
        GroupDetailsScreen(
            backStackEntry.arguments!!.getInt("courseId"),
            backStackEntry.arguments!!.getString("groupName")!!,
            onLogout = {
                navController.navigate(Screen.Login.navigationRoute) {
                    popUpTo(navController.graph.id) {
                        inclusive = true
                    }
                }
            },
            navController = navController
        )
    }
}

@RequiresApi(Build.VERSION_CODES.Q)
fun NavGraphBuilder.assignmentNavGraph(
    navController: NavHostController,
) {
    composable(
        route = Screen.AssignmentList.routePattern
    ) {
        AssignmentListScreen(
            null,
            null,
            onLogout = {
                navController.navigate(Screen.Login.navigationRoute) {
                    popUpTo(navController.graph.id) {
                        inclusive = true
                    }
                }
            },
            onListItemClick = { assignmentId ->
                navController.navigate(
                    "${Screen.AssignmentDetails.navigationRoute}/$assignmentId"
                )
            },
            navController = navController
        )
    }
    composable(
        route = Screen.AssignmentDetails.routePattern,
        arguments = listOf(
            navArgument("assignmentId") { type = NavType.IntType }
        )
    ) { backStackEntry ->
        AssignmentDetailsScreen(
            assignmentId = backStackEntry.arguments!!.getInt("assignmentId"),
            onLogout = {
                navController.navigate(Screen.Login.navigationRoute) {
                    popUpTo(navController.graph.id) {
                        inclusive = true
                    }
                }
            },
            navController = navController
        )
    }
}

fun NavGraphBuilder.appointmentNavGraph(
    navController: NavHostController
) {
    composable(
        route = Screen.AppointmentGrid.routePattern,
        arguments = listOf(
            navArgument("assignmentId") { type = NavType.IntType }
        )
    ) { backStackEntry ->
        AppointmentGridScreen(
            assignmentId = backStackEntry.arguments!!.getInt("assignmentId"),
            onLogout = {
                navController.navigate(Screen.Login.navigationRoute) {
                    popUpTo(navController.graph.id) {
                        inclusive = true
                    }
                }
            },
            navController = navController
        )
    }
}
