using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.BusinessLogic.Services.Interfaces;
using HomeworkManager.Model.Constants;
using HomeworkManager.Model.CustomEntities;
using HomeworkManager.Model.CustomEntities.Authentication;
using HomeworkManager.Model.CustomEntities.User;
using HomeworkManager.Model.Entities;
using HomeworkManager.Model.ErrorEntities;
using HomeworkManager.Model.ErrorEntities.Authentication;
using Microsoft.AspNetCore.Identity;

namespace HomeworkManager.BusinessLogic.Managers
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly IJwtService _jwtService;
        private readonly UserManager<User> _userManager;

        public AuthenticationManager(UserManager<User> userManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        public async Task<Result<UserModel, BusinessError>> RegisterAsync(UserModel newUser)
        {
            User identityUser = new() { UserName = newUser.UserName, Email = newUser.Email };

            var createResult = await _userManager.CreateAsync(
                identityUser,
                newUser.Password
            );

            if (!createResult.Succeeded)
            {
                return new BusinessError(createResult.Errors.Select(e => e.Description).ToArray());
            }

            var addToRoleResult = await _userManager.AddToRoleAsync(identityUser, Roles.Teacher);

            if (!addToRoleResult.Succeeded)
            {
                return new BusinessError(createResult.Errors.Select(e => e.Description).ToArray());
            }

            newUser.Password = "*****";
            return newUser;
        }

        public async Task<Result<AuthenticationResponse, BusinessError>> CreateBearerTokenAsync(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user is null)
            {
                return new BusinessError(AuthenticationErrorMessages.INVALID_USERNAME);
            }

            var validPassword = await _userManager.CheckPasswordAsync(user, password);

            if (!validPassword)
            {
                return new BusinessError(AuthenticationErrorMessages.INVALID_PASSWORD);
            }

            return await _jwtService.CreateTokensAsync(user);
        }

        public async Task<Result<AuthenticationResponse, BusinessError>
        > CreateRefreshTokenAsync(string accessToken, string refreshToken)
        {
            return await _jwtService.RefreshTokensAsync(accessToken, refreshToken);
        }
    }
}