using System.Net;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HomeworkManager.API.Attributes;

public class HomeworkManagerAuthorizeAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var identity = context.HttpContext.User.Identity;
        if (identity is { IsAuthenticated: false })
        {
            return;
        }

        var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
        var accessTokenRepository = context.HttpContext.RequestServices.GetRequiredService<IAccessTokenRepository>();

        var bearerToken = context.HttpContext.Request.Headers["Authorization"].First()!;
        var accessToken = bearerToken.Split(" ")[1];

        var username = context.HttpContext.User.Identity?.Name;

        if (username is not null)
        {
            var cancellationToken = context.HttpContext.RequestAborted;

            var user = await userRepository.GetByUsernameAsync(username, cancellationToken);

            if (user is not null)
            {
                var dbAccessToken = await accessTokenRepository.GetAsync(accessToken, user.Id);

                if (dbAccessToken is not null && dbAccessToken.IsActive)
                {
                    return;
                }
            }
        }

        context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
    }
}