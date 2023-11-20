package hu.bme.aut.android.homeworkmanagerapp.navigation

import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.runtime.Composable
import androidx.navigation.NavGraphBuilder
import androidx.navigation.NavHostController
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.navigation
import hu.bme.aut.android.homeworkmanagerapp.feature.auth.login.LoginScreen
import hu.bme.aut.android.homeworkmanagerapp.feature.auth.register.RegisterScreen

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
    }
}

@OptIn(ExperimentalMaterial3Api::class)
fun NavGraphBuilder.authNavGraph(
    navController: NavHostController,
) {
    navigation(
        startDestination = Screen.Login.route,
        route = AUTH_GRAPH_ROUTE,
    ) {
        composable(
            route = Screen.Login.route,
        ) {
            LoginScreen(
                onLogin = {
                    navController.navigate(Screen.CourseList.route) {
                        popUpTo(Screen.Login.route) {
                            inclusive = true
                        }
                    }
                },
                onRegisterClick = {
                    navController.navigate(Screen.Register.route) {
                        popUpTo(Screen.Login.route) {
                            inclusive = true
                        }
                    }
                },
            )
        }
        composable(route = Screen.Register.route) {
            RegisterScreen(
                onRegister = {
                    navController.navigate(Screen.Login.route) {
                        popUpTo(Screen.Register.route) {
                            inclusive = true
                        }
                    }
                },
                onLoginClick = {
                    navController.navigate(Screen.Login.route) {
                        popUpTo(Screen.Register.route) {
                            inclusive = true
                        }
                    }
                },
            )
        }
    }
}