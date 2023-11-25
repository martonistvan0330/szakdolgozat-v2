package hu.bme.aut.android.homeworkmanagerapp.domain.model.assignment

import java.text.SimpleDateFormat
import java.util.Date

data class AssignmentListRow(
    val assignmentId: Int,
    val name: String,
    val deadline: String,
    val isDraft: Boolean
)

data class AssignmentListRowWithDate(
    val assignmentId: Int,
    val name: String,
    val deadline: Date,
    val isDraft: Boolean
)

fun AssignmentListRow.withDate(): AssignmentListRowWithDate {
    val dateFormat = SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss")

    return AssignmentListRowWithDate(
        assignmentId = assignmentId,
        name = name,
        deadline = dateFormat.parse(deadline.split("+")[0])!!,
        isDraft = isDraft
    )
}