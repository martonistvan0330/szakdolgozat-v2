package hu.bme.aut.android.homeworkmanagerapp.domain.model.assignment

import hu.bme.aut.android.homeworkmanagerapp.domain.model.enums.AssignmentTypeId
import java.text.SimpleDateFormat
import java.util.Date

data class AssignmentModel(
    val assignmentId: Int,
    val name: String,
    val description: String?,
    val deadline: String,
    val presentationRequired: Boolean,
    val isDraft: Boolean,
    val assignmentTypeId: AssignmentTypeId,
    val assignmentTypeName: String?,
    val courseId: Int,
    val courseName: String,
    val groupName: String
)

data class AssignmentModelWithDate(
    val assignmentId: Int,
    val name: String,
    val description: String?,
    val deadline: Date,
    val presentationRequired: Boolean,
    val isDraft: Boolean,
    val assignmentTypeId: AssignmentTypeId,
    val assignmentTypeName: String?,
    val courseId: Int,
    val courseName: String,
    val groupName: String
)

fun AssignmentModel.withDate(): AssignmentModelWithDate {
    val dateFormat = SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss")

    return AssignmentModelWithDate(
        assignmentId = assignmentId,
        name = name,
        description = description,
        deadline = dateFormat.parse(deadline.split("+")[0])!!,
        presentationRequired = presentationRequired,
        isDraft = isDraft,
        assignmentTypeId = assignmentTypeId,
        assignmentTypeName = assignmentTypeName,
        courseId = courseId,
        courseName = courseName,
        groupName = groupName
    )
}