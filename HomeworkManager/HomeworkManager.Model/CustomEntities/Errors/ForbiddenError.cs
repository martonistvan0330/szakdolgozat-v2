using FluentResults;
using HomeworkManager.Model.Constants.Errors;

namespace HomeworkManager.Model.CustomEntities.Errors;

public class ForbiddenError : Error
{
    public ForbiddenError() : base()
    {
        WithMetadata("ErrorCode", ErrorCodes.FORBIDDEN);
    }
}