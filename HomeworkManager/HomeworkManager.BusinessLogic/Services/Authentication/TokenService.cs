using HomeworkManager.BusinessLogic.Services.Authentication.Interfaces;
using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.ErrorEntities;
using HomeworkManager.Model.ErrorEntities.Authentication;

namespace HomeworkManager.BusinessLogic.Services.Authentication;

public class TokenService : ITokenService
{
    private readonly IAccessTokenRepository _accessTokenRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public TokenService
    (
        IAccessTokenRepository accessTokenRepository,
        IRefreshTokenRepository refreshTokenRepository
    )
    {
        _accessTokenRepository = accessTokenRepository;
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
}