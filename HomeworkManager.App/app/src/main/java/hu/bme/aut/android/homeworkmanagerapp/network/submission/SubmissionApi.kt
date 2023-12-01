package hu.bme.aut.android.homeworkmanagerapp.network.submission

import hu.bme.aut.android.homeworkmanagerapp.domain.model.submission.FileSubmissionModel
import hu.bme.aut.android.homeworkmanagerapp.domain.model.submission.TextSubmissionModel
import hu.bme.aut.android.homeworkmanagerapp.domain.model.submission.UpdatedTextSubmission
import okhttp3.MultipartBody
import okhttp3.ResponseBody
import retrofit2.http.Body
import retrofit2.http.GET
import retrofit2.http.Multipart
import retrofit2.http.PATCH
import retrofit2.http.POST
import retrofit2.http.Part
import retrofit2.http.Path

interface SubmissionApi {
    @GET("/api/Submission/Text/{assignmentId}")
    suspend fun getTextSubmission(
        @Path("assignmentId") assignmentId: Int
    ): TextSubmissionModel?

    @POST("/api/Submission/Text")
    suspend fun upsertTextSubmission(
        @Body updatedTextSubmission: UpdatedTextSubmission
    ): Int

    @GET("/api/Submission/File/{assignmentId}")
    suspend fun getFileSubmission(
        @Path("assignmentId") assignmentId: Int
    ): FileSubmissionModel?

    @GET("/api/Submission/File/{assignmentId}/Download")
    suspend fun downloadFile(
        @Path("assignmentId") assignmentId: Int
    ): ResponseBody

    @Multipart
    @POST("/api/Submission/File/{assignmentId}")
    suspend fun uploadFile(
        @Path("assignmentId") assignmentId: Int,
        @Part file: MultipartBody.Part
    ): Int

    @PATCH("/api/Submission/{assignmentId}")
    suspend fun submit(
        @Path("assignmentId") assignmentId: Int
    )
}