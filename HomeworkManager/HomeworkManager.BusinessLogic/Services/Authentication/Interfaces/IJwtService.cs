using FluentResults;
using HomeworkManager.Model.CustomEntities.Authentication;
using HomeworkManager.Model.CustomEntities.User;
using HomeworkManager.Model.Entities;

namespace HomeworkManager.BusinessLogic.Services.Authentication.Interfaces;

public interface IJwtService
{
    Task<Result<AuthenticationResponse>> CreateTokensAsync(UserModel userModel, CancellationToken cancellationToken = default);
    Task<Result<AuthenticationResponse>> RefreshTokensAsync(RefreshRequest refreshRequest, CancellationToken cancellationToken = default);
}