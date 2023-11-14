using FluentResults;
using HomeworkManager.Model.Constants.Errors;

namespace HomeworkManager.Model.CustomEntities.Errors;

public class NotFoundError : Error
{
    public NotFoundError(string message) : base(message)
    {
        WithMetadata("ErrorCode", ErrorCodes.NOT_FOUND);
    }
}