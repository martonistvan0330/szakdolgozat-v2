using System.Net;
using System.Net.Mail;
using System.Text;
using HomeworkManager.API.Attributes;
using HomeworkManager.BusinessLogic.Managers.Interfaces;
using HomeworkManager.Model.CustomEntities.Authentication;
using HomeworkManager.Model.CustomEntities.User;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkManager.API.Controllers;

[ApiController]
[Route("api/Auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationManager _authenticationManager;

    public AuthController(
        IAuthenticationManager authenticationManager
    )
    {
        _authenticationManager = authenticationManager;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<AuthenticationResponse>> RegisterAsync(NewUser newUser)
    {
        var registerUserResult = await _authenticationManager.RegisterAsync(newUser);

        return registerUserResult.Match<ActionResult<AuthenticationResponse>>(
            result => Ok(result),
            error => BadRequest(error.Message)
        );
    }

    [HttpPost("ConfirmEmail")]
    public async Task<ActionResult<bool>> ConfirmEmailAsync(EmailConfirmationRequest emailConfirmationRequest)
    {
        var confirmEmailResult = await _authenticationManager.ConfirmEmailAsync(User.Identity?.Name, emailConfirmationRequest);

        return confirmEmailResult.Match<ActionResult<bool>>(
            result => Ok(result),
            error => BadRequest(error.Message)
        );
    }

    [HttpPost("Login")]
    public async Task<ActionResult<AuthenticationResponse>> CreateBearerTokenAsync(AuthenticationRequest authenticationRequest)
    {
        var authenticationResponseResult =
            await _authenticationManager.LoginAsync(authenticationRequest);

        return authenticationResponseResult.Match<ActionResult<AuthenticationResponse>>(
            result => Ok(result),
            error => Unauthorized(error.Message)
        );
    }

    [HttpPost("RefreshToken")]
    public async Task<ActionResult<AuthenticationResponse>> RefreshTokenAsync(RefreshRequest tokens)
    {
        var authenticationResult = await _authenticationManager.CreateRefreshTokenAsync(tokens.AccessToken, tokens.RefreshToken);

        return authenticationResult.Match<ActionResult<AuthenticationResponse>>(
            result => Ok(result),
            error => Unauthorized(error.Message)
        );
    }

    [HomeworkManagerAuthorize]
    [HttpPost("Logout")]
    public async Task<ActionResult<bool>> LogoutAsync(RevokeRequest tokens)
    {
        var logoutResult = await _authenticationManager.Logout(User.Identity?.Name, tokens);

        return logoutResult.Match<ActionResult<bool>>(
            result => Ok(result),
            error => Unauthorized(error.Message)
        );
    }

    [HomeworkManagerAuthorize]
    [HttpPatch("ResendConfirmation")]
    public async Task<ActionResult<bool>> ResendConfirmationEmailAsync()
    {
        var resendConfirmationResult = await _authenticationManager.ResendEmailConfirmationAsync(User.Identity?.Name);

        return resendConfirmationResult.Match<ActionResult<bool>>(
            result => Ok(result),
            error => BadRequest(error.Message)
        );
    }

    [HttpPost("PasswordRecovery")]
    public async Task<ActionResult> PasswordRecoveryAsync(PasswordRecoveryRequest passwordRecoveryRequest)
    {
        var smtpClient = new SmtpClient
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("noreply.hwm@gmail.com", "ngny aies brvq wdbv")
        };

        MailAddress from = new("noreply.hwm@gmail.com", "Homework Manager Admin", Encoding.UTF8);
        MailAddress to = new("mistvan0330@gmail.com");

        MailMessage message = new(from, to);
        message.Subject = "Test";
        message.SubjectEncoding = Encoding.UTF8;
        message.Body = "Hi!";
        message.BodyEncoding = Encoding.UTF8;

        await smtpClient.SendMailAsync(message);

        return Ok();
    }
}