using FluentResults;
using HomeworkManager.Model.Constants.Errors;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkManager.API.Extensions;

public static class ResultExtensions
{
    public static ActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess)
        {
            return new NoContentResult();
        }

        return result.Errors.ToActionResult();
    }

    public static ActionResult<T> ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return result.Value;
        }

        return result.Errors.ToActionResult();
    }

    private static ActionResult ToActionResult(this ICollection<IError> errors)
    {
        var errorCodes = errors
            .Select(error =>
            {
                error.Metadata.TryGetValue("ErrorCode", out var errorCode);
                return errorCode is not null ? (string)errorCode : string.Empty;
            })
            .ToHashSet();

        if (errorCodes.Contains(ErrorCodes.APPLICATION_ERROR))
        {
            throw new ApplicationException(errors.ToMessage());
        }

        if (errorCodes.Contains(ErrorCodes.FORBIDDEN))
        {
            return new ForbidResult();
        }

        if (errorCodes.Contains(ErrorCodes.NOT_FOUND))
        {
            return new NotFoundObjectResult(errors.ToMessages());
        }

        return new BadRequestObjectResult(errors.ToMessages());
    }

    private static IEnumerable<string> ToMessages(this IEnumerable<IError> errors)
    {
        return errors.Select(e => e.Message).ToList();
    }

    private static string ToMessage(this IEnumerable<IError> errors, string separator = "\n")
    {
        return string.Join(separator, errors.ToMessages());
    }
}