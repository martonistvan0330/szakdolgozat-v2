using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using HomeworkManager.BusinessLogic.Services.Interfaces;
using HomeworkManager.DataAccess.Repositories.Interfaces;
using HomeworkManager.Model.Configurations;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Authentication;
using HomeworkManager.Model.Entities;
using HomeworkManager.Model.ErrorEntities;
using HomeworkManager.Model.ErrorEntities.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HomeworkManager.BusinessLogic.Services
{
    public class JwtService : IJwtService
    {
        private const int EXPIRATION_MINUTES = 1;

        private readonly JwtConfiguration _jwtConfiguration;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly UserManager<User> _userManager;

        public JwtService(IOptions<JwtConfiguration> jwtConfiguration, UserManager<User> userManager, IRefreshTokenRepository refreshTokenRepository)
        {
            _jwtConfiguration = jwtConfiguration.Value;
            _userManager = userManager;
            _refreshTokenRepository = refreshTokenRepository;
        }


        public async Task<AuthenticationResponse> CreateTokensAsync(User user)
        {
            var expiration = DateTime.UtcNow.AddMinutes(EXPIRATION_MINUTES);

            var accessToken = CreateJwtAccessToken(
                await CreateClaimsAsync(user),
                CreateSigningCredentials(),
                expiration
            );

            var refreshToken = GenerateRefreshToken();

            await UpdateUserAsync(user, refreshToken);

            var tokenHandler = new JwtSecurityTokenHandler();

            return new AuthenticationResponse
            {
                AccessToken = tokenHandler.WriteToken(accessToken),
                RefreshToken = refreshToken,
                Expiration = expiration
            };
        }

        public async Task<Result<AuthenticationResponse, BusinessError>> RefreshTokensAsync(string accessToken, string refreshToken)
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

            var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var securityToken);

            if (principal?.Identity?.Name is null ||
                securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return new BusinessError(AuthenticationErrorMessages.INVALID_ACCESS_TOKEN);
            }

            var user = await _userManager.FindByNameAsync(principal.Identity.Name);

            if (user is null || !await _refreshTokenRepository.UserHasTokenAsync(user, refreshToken))
            {
                return new BusinessError(AuthenticationErrorMessages.INVALID_REFRESH_TOKEN);
            }

            return await CreateTokensAsync(user);
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

        private async Task<Claim[]> CreateClaimsAsync(User user)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, _jwtConfiguration.Subject),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName!),
                new(ClaimTypes.Email, user.Email!)
            };

            foreach (var role in await _userManager.GetRolesAsync(user))
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
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

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task UpdateUserAsync(User user, string refreshToken)
        {
            user.RefreshTokens.Add(new RefreshToken { Token = refreshToken, UserId = user.Id });

            await _userManager.UpdateAsync(user);
        }
    }
}