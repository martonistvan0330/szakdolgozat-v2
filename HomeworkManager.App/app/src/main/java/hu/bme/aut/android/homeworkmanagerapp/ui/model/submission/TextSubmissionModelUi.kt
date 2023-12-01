package hu.bme.aut.android.homeworkmanagerapp.ui.model.submission

import hu.bme.aut.android.homeworkmanagerapp.domain.model.submission.TextSubmissionModel

data class TextSubmissionModelUi(
    val submissionId: Int,
    val isDraft: Boolean,
    val answer: String
)

fun TextSubmissionModel.asTextSubmissionModelUi(): TextSubmissionModelUi {
    return TextSubmissionModelUi(
        submissionId = submissionId,
        isDraft = isDraft,
        answer = answer
    )
}