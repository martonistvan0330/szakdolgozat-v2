package hu.bme.aut.android.homeworkmanagerapp.ui.model.submission

import hu.bme.aut.android.homeworkmanagerapp.domain.model.submission.FileSubmissionModel

data class FileSubmissionModelUi(
    val submissionId: Int,
    val isDraft: Boolean,
    val fileName: String?
)

fun FileSubmissionModel.asFileSubmissionModelUi(): FileSubmissionModelUi {
    return FileSubmissionModelUi(
        submissionId = submissionId,
        isDraft = isDraft,
        fileName = fileName
    )
}