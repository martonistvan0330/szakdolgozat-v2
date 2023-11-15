namespace HomeworkManager.Model.Constants.Errors.Authentication;

public static class AuthenticationErrorMessages
{
    public const string USER_NOT_FOUND = "User not found!";
    public const string USER_EMAIL_ALREADY_CONFIRMED = "Email has been confirmed before";
    public const string INVALID_USERNAME = "Invalid username or password!";
    public const string INVALID_PASSWORD = "Invalid username or password!";
    public const string INVALID_ACCESS_TOKEN = "Invalid access token!";
    public const string INVALID_REFRESH_TOKEN = "Invalid refresh token!";
    public const string INVALID_TOKEN = "Invalid token!";
    public const string FORBIDDEN = "You are not allowed!";
    public const string TOKEN_CREATION_FAILED = "Token creation failed!";
    public const string TOKEN_REVOCATION_FAILED = "Token revocation failed!";
}