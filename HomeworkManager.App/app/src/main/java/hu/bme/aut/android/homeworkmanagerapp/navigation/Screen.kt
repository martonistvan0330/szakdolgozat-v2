package hu.bme.aut.android.homeworkmanagerapp.navigation

const val AUTH_GRAPH_ROUTE = "auth"
const val ASSIGNMENT_GRAPH_ROUTE = "assignments"
const val COURSE_GRAPH_ROUTE = "courses"
const val GROUP_GRAPH_ROUTE = "groups"
const val APPOINTMENT_GRAPH_ROUTE = "appointments"

sealed class Screen(val routePattern: String, val navigationRoute: String = routePattern) {
    object Login : Screen(routePattern = "$AUTH_GRAPH_ROUTE/login")
    object Register : Screen(routePattern = "$AUTH_GRAPH_ROUTE/register")
    object CourseList : Screen(routePattern = COURSE_GRAPH_ROUTE)
    object GroupList : Screen(
        routePattern = "$COURSE_GRAPH_ROUTE/{courseId}/$GROUP_GRAPH_ROUTE",
        navigationRoute = GROUP_GRAPH_ROUTE
    )

    object GroupDetails : Screen(
        routePattern = "$COURSE_GRAPH_ROUTE/{courseId}/$GROUP_GRAPH_ROUTE/{groupName}",
        navigationRoute = GROUP_GRAPH_ROUTE
    )

    object AssignmentList : Screen(routePattern = ASSIGNMENT_GRAPH_ROUTE)
    object AssignmentDetails : Screen(
        routePattern = "$ASSIGNMENT_GRAPH_ROUTE/{assignmentId}",
        navigationRoute = ASSIGNMENT_GRAPH_ROUTE
    )
    object AppointmentGrid : Screen(
        routePattern = "$APPOINTMENT_GRAPH_ROUTE/{assignmentId}",
        navigationRoute = APPOINTMENT_GRAPH_ROUTE
    )
}