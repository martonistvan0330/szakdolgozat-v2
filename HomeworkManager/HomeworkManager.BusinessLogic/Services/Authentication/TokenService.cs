using System.Security.Cryptography;
using System.Transactions;
using FluentResults;
using HomeworkManager.BusinessLogic.Services.Authentication.Interfaces;
using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.Constants.Errors.Authentication;
using HomeworkManager.Model.CustomEntities.Errors;

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

    public async Task<Result> CheckTokensAsync(string accessToken, string refreshToken, Guid userId, CancellationToken cancellationToken = default)
    {
        var dbAccessToken = await _accessTokenRepository.GetAsync(accessToken, userId, cancellationToken);

        if (dbAccessToken is null || !dbAccessToken.IsActive)
        {
            return new BusinessError(AuthenticationErrorMessages.INVALID_ACCESS_TOKEN);
        }

        var dbRefreshToken = await _refreshTokenRepository.GetAsync(refreshToken, dbAccessToken, cancellationToken);

        if (dbRefreshToken is null || !dbRefreshToken.IsActive)
        {
            return new BusinessError(AuthenticationErrorMessages.INVALID_REFRESH_TOKEN);
        }

        return Result.Ok();
    }

    public async Task<Result> AddTokensToUserAsync(string accessToken, string refreshToken, Guid userId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var transactionScope = new TransactionScope();
            
            var dbAccessToken = await _accessTokenRepository.CreateAsync(accessToken, userId, cancellationToken);
            await _refreshTokenRepository.CreateAsync(refreshToken, dbAccessToken.AccessTokenId, cancellationToken);

            transactionScope.Complete();
            
            return Result.Ok();
        }
        catch
        {
            return new ApplicationError(AuthenticationErrorMessages.TOKEN_CREATION_FAILED);
        }
    }

    public async Task<Result> RevokeTokensAsync(string accessToken, string refreshToken, Guid userId, CancellationToken cancellationToken = default)
    {
        using var transactionScope = new TransactionScope();
        
        var dbAccessToken = await _accessTokenRepository.RevokeAsync(accessToken, userId, cancellationToken);
        if (dbAccessToken is null)
        {
            return new ApplicationError(AuthenticationErrorMessages.TOKEN_REVOCATION_FAILED);
        }
        
        await _refreshTokenRepository.RevokeAsync(refreshToken, dbAccessToken, cancellationToken);
        transactionScope.Complete();
        
        return Result.Ok();
    }

    public async Task<Result<string>> CreateEmailConfirmationTokenAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var emailConfirmationToken = GenerateToken();

        return await _emailConfirmationTokenRepository.CreateAsync(userId, emailConfirmationToken, cancellationToken);
    }

    public async Task<Result<string>> CreatePasswordRecoveryTokenAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var passwordRecoveryToken = GenerateToken();
        var dbPasswordRecoveryToken =  await _passwordRecoveryTokenRepository.CreateAsync(userId, passwordRecoveryToken, cancellationToken);

        return dbPasswordRecoveryToken is null
            ? new BusinessError(AuthenticationErrorMessages.TOKEN_CREATION_FAILED)
            : dbPasswordRecoveryToken.Token;
    }

    public async Task<Result> CheckEmailConfirmationTokenAsync(Guid userId, string emailConfirmationToken,
        CancellationToken cancellationToken = default)
    {
        var dbEmailConfirmationToken = await _emailConfirmationTokenRepository.GetActiveByUserIdAsync(userId, cancellationToken);

        if (dbEmailConfirmationToken is null || dbEmailConfirmationToken.Token != emailConfirmationToken)
        {
            return Result.Fail(new BusinessError(AuthenticationErrorMessages.INVALID_TOKEN));
        }

        var success = await _emailConfirmationTokenRepository.RevokeAsync(dbEmailConfirmationToken, cancellationToken);

        if (!success)
        {
            return Result.Fail(new BusinessError(AuthenticationErrorMessages.INVALID_TOKEN));
        }

        return Result.Ok();
    }

    public async Task<Result<Guid>> GetUserIdByPasswordRecoveryTokenAsync(string passwordRecoveryToken,
        CancellationToken cancellationToken = default)
    {
        var userId = await _passwordRecoveryTokenRepository.GetUserIdByActiveTokenAsync(passwordRecoveryToken, cancellationToken);

        return userId is null
            ? new BusinessError(AuthenticationErrorMessages.INVALID_TOKEN)
            : (Guid)userId;
    }

    public async Task<Result> RevokePasswordRecoveryTokenAsync(string passwordRecoveryToken, CancellationToken cancellationToken = default)
    {
        await _passwordRecoveryTokenRepository.RevokeAsync(passwordRecoveryToken, cancellationToken);

        return Result.Ok();
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