package hu.bme.aut.android.homeworkmanagerapp.ui.model.assignment

import hu.bme.aut.android.homeworkmanagerapp.domain.model.assignment.AssignmentModelWithDate
import hu.bme.aut.android.homeworkmanagerapp.domain.model.enums.AssignmentTypeId
import java.text.SimpleDateFormat

data class AssignmentModelUi(
    val name: String,
    val description: String?,
    val deadline: String,
    val presentationRequired: Boolean,
    val isDraft: Boolean,
    val assignmentTypeId: AssignmentTypeId,
    val courseName: String,
    val groupName: String
)

fun AssignmentModelWithDate.asAssignmentListRowUi(): AssignmentModelUi {
    val dateFormat = SimpleDateFormat("yyyy-MM-dd HH:mm:ss")

    return AssignmentModelUi(
        name = name,
        description = description,
        deadline = dateFormat.format(deadline),
        presentationRequired = presentationRequired,
        isDraft = isDraft,
        assignmentTypeId = assignmentTypeId,
        courseName = courseName,
        groupName = groupName
    )
}