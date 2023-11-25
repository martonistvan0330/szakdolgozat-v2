package hu.bme.aut.android.homeworkmanagerapp.ui.model.assignment

import hu.bme.aut.android.homeworkmanagerapp.domain.model.assignment.AssignmentListRow
import hu.bme.aut.android.homeworkmanagerapp.domain.model.assignment.AssignmentListRowWithDate
import java.text.SimpleDateFormat

data class AssignmentListRowUi(
    val assignmentId: Int = 0,
    val name: String = "",
    val deadline: String = "",
    val isDraft: Boolean = false
)

fun AssignmentListRowWithDate.asAssignmentListRowUi(): AssignmentListRowUi {
    val dateFormat = SimpleDateFormat("yyyy-MM-dd HH:mm:ss")

    return AssignmentListRowUi(
        assignmentId = assignmentId,
        name = name,
        deadline = dateFormat.format(deadline),
        isDraft = isDraft
    )
}

fun AssignmentListRowUi.asAssignmentListRow(): AssignmentListRow {
    return AssignmentListRow(
        assignmentId = assignmentId,
        name = name,
        deadline = deadline,
        isDraft = isDraft
    )
}