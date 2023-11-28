using FluentResults;
using HomeworkManager.Model.Constants.Errors;

namespace HomeworkManager.Model.CustomEntities.Errors;

public class ConflictError : Error
{
    public ConflictError(string message) : base(message)
    {
        WithMetadata("ErrorCode", ErrorCodes.CONFLICT);
    }
}