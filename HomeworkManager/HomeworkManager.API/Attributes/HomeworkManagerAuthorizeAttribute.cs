using System.Net;
using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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

        var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<User>>();
        var accessTokenRepository = context.HttpContext.RequestServices.GetRequiredService<IAccessTokenRepository>();

        var bearerToken = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();

        if (bearerToken is not null)
        {
            var tokenParts = bearerToken.Split(" ");

            if (tokenParts is ["Bearer", _, ..])
            {
                var accessToken = tokenParts[1];

                var username = context.HttpContext.User.Identity?.Name;

                if (username is not null)
                {
                    var user = await userManager.FindByNameAsync(username);

                    if (user is not null)
                    {
                        var dbAccessToken = await accessTokenRepository.GetAsync(accessToken, user);

                        if (dbAccessToken is not null && dbAccessToken.IsActive)
                        {
                            return;
                        }
                    }
                }
            }
        }

        context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
    }
}