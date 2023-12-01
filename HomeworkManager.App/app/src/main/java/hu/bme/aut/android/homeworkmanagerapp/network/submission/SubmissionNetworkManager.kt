package hu.bme.aut.android.homeworkmanagerapp.network.submission

import hu.bme.aut.android.homeworkmanagerapp.domain.model.submission.FileSubmissionModel
import hu.bme.aut.android.homeworkmanagerapp.domain.model.submission.TextSubmissionModel
import hu.bme.aut.android.homeworkmanagerapp.domain.model.submission.UpdatedTextSubmission
import okhttp3.MultipartBody
import okhttp3.RequestBody
import javax.inject.Inject

class SubmissionNetworkManager @Inject constructor(
    private val submissionApi: SubmissionApi
) {
    suspend fun getTextSubmission(assignmentId: Int): TextSubmissionModel? {
        return try {
            submissionApi.getTextSubmission(assignmentId)
        } catch (nullPointerException: KotlinNullPointerException) {
            null
        }
    }

    suspend fun upsertTextSubmission(updatedTextSubmission: UpdatedTextSubmission): Int {
        return submissionApi.upsertTextSubmission(updatedTextSubmission)
    }

    suspend fun getFileSubmission(assignmentId: Int): FileSubmissionModel? {
        return try {
            submissionApi.getFileSubmission(assignmentId)
        } catch (nullPointerException: KotlinNullPointerException) {
            null
        }
    }

    suspend fun downloadFile(assignmentId: Int): ByteArray {
        return submissionApi.downloadFile(assignmentId).bytes()
    }

    suspend fun uploadFile(
        assignmentId: Int,
        fileName: String,
        requestFile: RequestBody
    ): Int {
        val formData = MultipartBody.Part.createFormData("submission", fileName, requestFile)
        return submissionApi.uploadFile(assignmentId, formData)
    }

    suspend fun submit(assignmentId: Int) {
        submissionApi.submit(assignmentId)
    }
}