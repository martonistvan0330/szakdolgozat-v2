package hu.bme.aut.android.homeworkmanagerapp.navigation

const val AUTH_GRAPH_ROUTE = "auth"
const val COURSE_GRAPH_ROUTE = "courses"
const val GROUP_GRAPH_ROUTE = "groups"

sealed class Screen(val routePattern: String, val navigationRoute: String = routePattern) {
    object Login : Screen(routePattern = "$AUTH_GRAPH_ROUTE/login")
    object Register : Screen(routePattern = "$AUTH_GRAPH_ROUTE/register")
    object CourseList : Screen(routePattern = COURSE_GRAPH_ROUTE)
    object GroupList : Screen(
        routePattern = "$COURSE_GRAPH_ROUTE/{courseId}/$GROUP_GRAPH_ROUTE",
        navigationRoute = "$GROUP_GRAPH_ROUTE"
    )
}