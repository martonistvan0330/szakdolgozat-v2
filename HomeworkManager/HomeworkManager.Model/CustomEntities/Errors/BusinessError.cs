using FluentResults;
using HomeworkManager.Model.Constants.Errors;

namespace HomeworkManager.Model.CustomEntities.Errors;

public class BusinessError : Error
{
    public BusinessError(string message) : base(message)
    {
        WithMetadata("ErrorCode", ErrorCodes.BUSINESS_ERROR);
    }
}