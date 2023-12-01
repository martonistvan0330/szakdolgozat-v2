package hu.bme.aut.android.homeworkmanagerapp.domain.model.enums

import com.google.gson.annotations.SerializedName

enum class AssignmentTypeId {
    @SerializedName("1")
    TextAnswerAssignment,

    @SerializedName("2")
    FileUploadAssignment
}