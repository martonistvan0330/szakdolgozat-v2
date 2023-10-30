using System.Security.Cryptography;
using HomeworkManager.BusinessLogic.Services.Authentication.Interfaces;
using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.Constants.Errors.Authentication;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.ErrorEntities;

namespace HomeworkManager.BusinessLogic.Services.Authentication;

public class TokenService : ITokenService
{
    private readonly IAccessTokenRepository _accessTokenRepository;
    private readonly IEmailConfirmationTokenRepository _emailConfirmationTokenRepository;
    private readonly IPasswordRecoveryTokenRepository _passwordRecoveryTokenRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public TokenService
    (
        IAccessTokenRepository accessTokenRepository,
        IEmailConfirmationTokenRepository emailConfirmationTokenRepository,
        IPasswordRecoveryTokenRepository passwordRecoveryTokenRepository,
        IRefreshTokenRepository refreshTokenRepository
    )
    {
        _accessTokenRepository = accessTokenRepository;
        _emailConfirmationTokenRepository = emailConfirmationTokenRepository;
        _passwordRecoveryTokenRepository = passwordRecoveryTokenRepository;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<Result<bool, BusinessError>> CheckTokensAsync(string accessToken, string refreshToken, Guid userId)
    {
        var dbAccessToken = await _accessTokenRepository.GetAsync(accessToken, userId);

        if (dbAccessToken is null || !dbAccessToken.IsActive)
        {
            return new BusinessError(AuthenticationErrorMessages.INVALID_ACCESS_TOKEN);
        }

        var dbRefreshToken = await _refreshTokenRepository.GetAsync(refreshToken, dbAccessToken);

        if (dbRefreshToken is null || !dbRefreshToken.IsActive)
        {
            return new BusinessError(AuthenticationErrorMessages.INVALID_REFRESH_TOKEN);
        }

        return true;
    }

    public async Task AddTokensToUserAsync(string accessToken, string refreshToken, Guid userId)
    {
        await _accessTokenRepository.CreateAsync(accessToken, refreshToken, userId);
    }

    public async Task RevokeTokensAsync(string accessToken, string refreshToken, Guid userId)
    {
        var dbAccessToken = await _accessTokenRepository.RevokeAsync(accessToken, userId);
        if (dbAccessToken is not null)
        {
            await _refreshTokenRepository.RevokeAsync(refreshToken, dbAccessToken);
        }
    }

    public async Task<string?> CreateEmailConfirmationTokenAsync(Guid userId)
    {
        var emailConfirmationToken = GenerateToken();
        return await _emailConfirmationTokenRepository.CreateAsync(userId, emailConfirmationToken);
    }

    public async Task<Result<bool, BusinessError>> CheckEmailConfirmationTokenAsync(Guid userId, string emailConfirmationToken)
    {
        var dbEmailConfirmationToken = await _emailConfirmationTokenRepository.GetActiveByUserAsync(userId);

        if (dbEmailConfirmationToken is null)
        {
            return new BusinessError(AuthenticationErrorMessages.INVALID_EMAIL_CONFIRMATION_TOKEN);
        }

        if (emailConfirmationToken == dbEmailConfirmationToken.Token)
        {
            await _emailConfirmationTokenRepository.RevokeAsync(dbEmailConfirmationToken);
            return true;
        }

        return false;
    }

    private static string GenerateToken()
    {
        var randomNumber = new byte[64];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber)
            .Replace("/", "")
            .Replace("&", "")
            .Replace("?", "")
            .Replace("=", "")
            .Replace(":", "")
            .Replace("%", "")
            .Replace("+", "");
    }
}