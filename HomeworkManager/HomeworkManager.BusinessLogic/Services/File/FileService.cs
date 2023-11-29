using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FluentResults;
using HomeworkManager.BusinessLogic.Services.File.Interfaces;
using HomeworkManager.Model.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace HomeworkManager.BusinessLogic.Services.File;

public class FileService : IFileService
{
    private readonly BlobContainerClient _blobContainerClient;

    public FileService(IOptions<BlobStorageConfiguration> fileStorageConfiguration)
    {
        var blobServiceClient = new BlobServiceClient(fileStorageConfiguration.Value.ConnectionString);
        _blobContainerClient = blobServiceClient.GetBlobContainerClient(fileStorageConfiguration.Value.ContainerName);
    }

    public async Task<Result<Stream>> DownloadSubmissionAsync(int assignmentId, Guid studentId, string fileName,
        CancellationToken cancellationToken = default)
    {
        var blobClient = _blobContainerClient.GetBlobClient(fileName);

        return await blobClient.OpenReadAsync(cancellationToken: cancellationToken);
    }

    public async Task<string> UploadSubmissionAsync(int assignmentId, IFormFile submission, Guid studentId,
        CancellationToken cancellationToken = default)
    {
        string fileExtension = submission.FileName.Split(".")[1];
        string fileName = $"Assignment_{assignmentId}_{studentId}.{fileExtension}";

        var blobClient = _blobContainerClient.GetBlobClient(fileName);

        await using var stream = new MemoryStream();

        await submission.CopyToAsync(stream, cancellationToken);
        stream.Seek(0, SeekOrigin.Begin);

        await blobClient.UploadAsync(stream, true, cancellationToken);

        await blobClient.SetHttpHeadersAsync(new BlobHttpHeaders
        {
            ContentType = submission.ContentType
        }, cancellationToken: cancellationToken);

        return fileName;
    }
}