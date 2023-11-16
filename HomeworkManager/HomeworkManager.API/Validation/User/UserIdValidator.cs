using FluentValidation;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.Model.Constants;
using HomeworkManager.Model.Constants.Errors;
using HomeworkManager.Model.Constants.Errors.User;

namespace HomeworkManager.API.Validation.User;

public class UserIdValidator : AbstractValidator<Guid>
{
    public const string IS_USER = nameof(IS_USER);

    public UserIdValidator(IUserManager userManager)
    {
        RuleFor(x => x)
            .MustAsync(async (userId, cancellationToken) =>
                await userManager.ExistsByIdAsync(userId, cancellationToken))
            .WithMessage(UserErrorMessages.USER_WITH_ID_NOT_FOUND);

        RuleSet(IS_USER, () =>
        {
            RuleFor(x => x)
                .MustAsync(async (userId, cancellationToken) =>
                {
                    if (await userManager.CurrentUserHasRoleAsync(Roles.ADMINISTRATOR, cancellationToken))
                    {
                        return true;
                    }

                    var currentUserId = await userManager.GetCurrentUserIdAsync(cancellationToken);
                    return userId == currentUserId;
                })
                .WithErrorCode(ErrorCodes.FORBIDDEN);
        });
    }
}