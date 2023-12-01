package hu.bme.aut.android.homeworkmanagerapp.feature.submission

import android.os.Build
import androidx.annotation.RequiresApi
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import hu.bme.aut.android.homeworkmanagerapp.domain.model.enums.AssignmentTypeId
import hu.bme.aut.android.homeworkmanagerapp.feature.submission.file.FileSubmissionDetails
import hu.bme.aut.android.homeworkmanagerapp.feature.submission.text.TextSubmissionDetails

@RequiresApi(Build.VERSION_CODES.Q)
@Composable
fun SubmissionDetails(
    assignmentId: Int,
    assignmentTypeId: AssignmentTypeId,
    modifier: Modifier = Modifier
) {
    when (assignmentTypeId) {
        AssignmentTypeId.TextAnswerAssignment -> TextSubmissionDetails(assignmentId, modifier)

        AssignmentTypeId.FileUploadAssignment -> FileSubmissionDetails(assignmentId, modifier)
    }
}