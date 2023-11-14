using FluentValidation.Results;
using HomeworkManager.Model.Constants.Errors;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkManager.API.Extensions;

public static class ValidationExtensions
{
    public static ActionResult ToActionResult(this ValidationResult validationResult)
    {
        var errorCodes = validationResult.Errors
            .Select(error => error.ErrorCode)
            .ToHashSet();

        if (errorCodes.Contains(ErrorCodes.FORBIDDEN))
        {
            return new ForbidResult();
        }

        return new BadRequestObjectResult(validationResult.ToString());
    }
}