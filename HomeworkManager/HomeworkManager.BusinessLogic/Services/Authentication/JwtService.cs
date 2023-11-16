using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;
using FluentResults;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.BusinessLogic.Services.Authentication.Interfaces;
using HomeworkManager.Model.Configurations;
using HomeworkManager.Model.Constants.Errors.Authentication;
using HomeworkManager.Model.CustomEntities.Authentication;
using HomeworkManager.Model.CustomEntities.Errors;
using HomeworkManager.Model.CustomEntities.User;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HomeworkManager.BusinessLogic.Services.Authentication;

public class JwtService : IJwtService
{
    private readonly JwtConfiguration _jwtConfiguration;
    private readonly ITokenService _tokenService;
    private readonly IUserManager _userManager;

    public JwtService(
        IOptions<JwtConfiguration> jwtConfiguration,
        ITokenService tokenService,
        IUserManager userManager
    )
    {
        _jwtConfiguration = jwtConfiguration.Value;
        _tokenService = tokenService;
        _userManager = userManager;
    }


    public async Task<Result<AuthenticationResponse>> CreateTokensAsync(UserModel userModel, CancellationToken cancellationToken = default)
    {
        var expiration = DateTime.UtcNow.AddMinutes(_jwtConfiguration.ExpirationMinutes);

        var accessJwt = CreateJwtAccessToken(
            CreateClaimsAsync(userModel),
            CreateSigningCredentials(),
            expiration
        );

        var refreshToken = GenerateRefreshToken();

        var tokenHandler = new JwtSecurityTokenHandler();

        var accessToken = tokenHandler.WriteToken(accessJwt);

        await _tokenService.AddTokensToUserAsync(accessToken, refreshToken, userModel.UserId, cancellationToken);

        return new AuthenticationResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Expiration = expiration
        };
    }

    public async Task<Result<AuthenticationResponse>> RefreshTokensAsync(RefreshRequest refreshRequest, CancellationToken cancellationToken = default)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Key)),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var principal = tokenHandler.ValidateToken(refreshRequest.AccessToken, tokenValidationParameters, out var securityToken);

        if (principal?.Identity?.Name is null ||
            securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            return Result.Fail(new BusinessError(AuthenticationErrorMessages.INVALID_ACCESS_TOKEN));
        }

        var userModelResult = await _userManager.GetModelByUsernameAsync(principal.Identity.Name, cancellationToken);

        if (!userModelResult.IsSuccess)
        {
            return new BusinessError(AuthenticationErrorMessages.INVALID_REFRESH_TOKEN);
        }

        var userModel = userModelResult.Value;

        var checkTokensResult =
            await _tokenService.CheckTokensAsync(refreshRequest.AccessToken, refreshRequest.RefreshToken, userModel.UserId, cancellationToken);

        if (!checkTokensResult.IsSuccess)
        {
            return checkTokensResult.ToResult<AuthenticationResponse>();
        }

        using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        await _tokenService.RevokeTokensAsync(refreshRequest.AccessToken, refreshRequest.RefreshToken, userModel.UserId, cancellationToken);

        var tokenResult = await CreateTokensAsync(userModel, cancellationToken);

        transactionScope.Complete();

        return tokenResult;
    }

    private JwtSecurityToken CreateJwtAccessToken(Claim[] claims, SigningCredentials credentials, DateTime expiration)
    {
        return new JwtSecurityToken(
            _jwtConfiguration.Issuer,
            _jwtConfiguration.Audience,
            claims,
            expires: expiration,
            signingCredentials: credentials
        );
    }

    private Claim[] CreateClaimsAsync(UserModel userModel)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, _jwtConfiguration.Subject),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
            new(ClaimTypes.NameIdentifier, userModel.UserId.ToString()),
            new(ClaimTypes.Name, userModel.Username),
            new(ClaimTypes.Email, userModel.Email)
        };

        foreach (var role in userModel.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Name));
        }

        return claims.ToArray();
    }

    private SigningCredentials CreateSigningCredentials()
    {
        return new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Key)),
            SecurityAlgorithms.HmacSha256
        );
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}