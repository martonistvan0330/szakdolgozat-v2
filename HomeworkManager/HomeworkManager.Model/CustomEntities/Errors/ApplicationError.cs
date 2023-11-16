using FluentResults;
using HomeworkManager.Model.Constants.Errors;

namespace HomeworkManager.Model.CustomEntities.Errors;

public class ApplicationError : Error
{
    public ApplicationError(string message) : base(message)
    {
        WithMetadata("ErrorCode", ErrorCodes.APPLICATION_ERROR);
    }
}