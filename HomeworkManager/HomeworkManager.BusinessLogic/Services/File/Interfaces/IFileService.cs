using FluentResults;
using Microsoft.AspNetCore.Http;

namespace HomeworkManager.BusinessLogic.Services.File.Interfaces;

public interface IFileService
{
    Task<Result<Stream>> DownloadSubmissionAsync(int assignmentId, Guid studentId, string fileName, CancellationToken cancellationToken = default);
    Task<string> UploadSubmissionAsync(int assignmentId, IFormFile submission, Guid studentId, CancellationToken cancellationToken = default);
}