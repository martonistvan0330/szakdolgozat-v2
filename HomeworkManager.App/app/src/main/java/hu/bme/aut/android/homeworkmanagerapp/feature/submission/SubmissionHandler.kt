package hu.bme.aut.android.homeworkmanagerapp.feature.submission

import hu.bme.aut.android.homeworkmanagerapp.domain.model.submission.FileSubmissionModel
import hu.bme.aut.android.homeworkmanagerapp.domain.model.submission.TextSubmissionModel
import hu.bme.aut.android.homeworkmanagerapp.domain.model.submission.UpdatedTextSubmission
import hu.bme.aut.android.homeworkmanagerapp.network.RefreshService
import hu.bme.aut.android.homeworkmanagerapp.network.handleAuthorizedRequest
import hu.bme.aut.android.homeworkmanagerapp.network.submission.SubmissionNetworkManager
import okhttp3.MediaType.Companion.toMediaType
import okhttp3.RequestBody.Companion.toRequestBody
import retrofit2.HttpException
import javax.inject.Inject

class SubmissionHandler @Inject constructor(
    private val submissionNetworkManager: SubmissionNetworkManager,
    private val refreshService: RefreshService
) {
    suspend fun getTextAssignment(
        assignmentId: Int,
        onSuccess: (TextSubmissionModel) -> Unit,
        onError: () -> Unit
    ) {
        try {
            var submission = handleAuthorizedRequest(refreshService) {
                submissionNetworkManager.getTextSubmission(assignmentId)
            }

            if (submission == null) {
                submission = TextSubmissionModel()
            }

            onSuccess(submission)
        } catch (httpException: HttpException) {
            onError()
        }
    }

    suspend fun upsertTextAssignment(
        updatedTextSubmission: UpdatedTextSubmission,
        onSuccess: (Int) -> Unit,
        onError: () -> Unit
    ) {
        try {
            val submissionId = handleAuthorizedRequest(refreshService) {
                submissionNetworkManager.upsertTextSubmission(updatedTextSubmission)
            }

            onSuccess(submissionId)
        } catch (httpException: HttpException) {
            onError()
        }
    }

    suspend fun getFileAssignment(
        assignmentId: Int,
        onSuccess: (FileSubmissionModel) -> Unit,
        onError: () -> Unit
    ) {
        try {
            var submission = handleAuthorizedRequest(refreshService) {
                submissionNetworkManager.getFileSubmission(assignmentId)
            }
            if (submission == null) {
                submission = FileSubmissionModel()
            }

            onSuccess(submission)
        } catch (httpException: HttpException) {
            onError()
        }
    }

    suspend fun downloadFile(
        assignmentId: Int,
        onSuccess: (ByteArray) -> Unit,
        onError: () -> Unit
    ) {
        try {
            val bytes = handleAuthorizedRequest(refreshService) {
                submissionNetworkManager.downloadFile(assignmentId)
            }

            onSuccess(bytes)
        } catch (httpException: HttpException) {
            onError()
        }
    }

    suspend fun uploadFile(
        assignmentId: Int,
        fileName: String,
        bytes: ByteArray,
        onSuccess: (Int) -> Unit,
        onError: () -> Unit
    ) {
        try {
            val submissionId = handleAuthorizedRequest(refreshService) {
                submissionNetworkManager.uploadFile(
                    assignmentId,
                    fileName,
                    bytes.toRequestBody("application/octet-stream".toMediaType())
                )
            }

            onSuccess(submissionId)
        } catch (httpException: HttpException) {
            onError()
        }
    }

    suspend fun submit(
        assignmentId: Int,
        onSuccess: () -> Unit,
        onError: () -> Unit
    ) {
        try {
            handleAuthorizedRequest(refreshService) {
                submissionNetworkManager.submit(assignmentId)
            }

            onSuccess()
        } catch (nullPointerException: KotlinNullPointerException) {
            onSuccess()
        } catch (httpException: HttpException) {
            onError()
        }
    }
}