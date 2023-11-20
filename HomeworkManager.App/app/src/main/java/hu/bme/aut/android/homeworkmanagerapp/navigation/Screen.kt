package hu.bme.aut.android.homeworkmanagerapp.navigation

const val ROOT_GRAPH_ROUTE = "root"
const val AUTH_GRAPH_ROUTE = "auth"

sealed class Screen(val route: String) {
    object Login : Screen(route = "login")
    object Register : Screen(route = "register")
    object CourseList : Screen(route = "courses")
}