﻿using System.Net;
using HomeworkManager.DataAccess.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HomeworkManager.API.Attributes;

public class HomeworkManagerAuthorizeAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    public HomeworkManagerAuthorizeAttribute(params string[] roles)
    {
        if (roles.Length > 0)
        {
            Roles = string.Join(',', roles);
        }
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var identity = context.HttpContext.User.Identity;
        if (identity is { IsAuthenticated: false })
        {
            return;
        }

        var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
        var accessTokenRepository = context.HttpContext.RequestServices.GetRequiredService<IAccessTokenRepository>();

        string bearerToken = context.HttpContext.Request.Headers["Authorization"].First()!;
        string accessToken = bearerToken.Split(" ")[1];

        string? username = context.HttpContext.User.Identity?.Name;

        if (username is not null)
        {
            var cancellationToken = context.HttpContext.RequestAborted;

            var user = await userRepository.GetByUsernameAsync(username, cancellationToken);

            if (user is not null)
            {
                var dbAccessToken = await accessTokenRepository.GetAsync(accessToken, user.Id, cancellationToken);

                if (dbAccessToken is not null && dbAccessToken.IsActive)
                {
                    return;
                }
            }
        }

        context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
    }
}