package hu.bme.aut.android.homeworkmanagerapp.domain.model.submission

data class TextSubmissionModel(
    val submissionId: Int = 0,
    val studentName: String = "",
    val submittedAt: String? = null,
    val isDraft: Boolean = true,
    val answer: String = ""
)