using FluentValidation;
using HomeworkManager.BusinessLogic.Managers.Interfaces;

namespace HomeworkManager.API.Validation.Role;

public class RoleValidator : AbstractValidator<IEnumerable<int>>
{
    public RoleValidator(IRoleManager roleManager)
    {
        RuleForEach(x => x)
            .NotNull()
            .MustAsync(async (roleId, cancellationToken) =>
                await roleManager.ExistsByIdAsync(roleId, cancellationToken));
    }
}