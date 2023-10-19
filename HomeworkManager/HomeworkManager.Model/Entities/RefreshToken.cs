namespace HomeworkManager.Model.Entities
{
    public class RefreshToken
    {
        public int RefreshTokenId { get; set; }
        public required string Token { get; set; }
        public bool IsActive { get; set; } = true;
        
        public int AccessTokenId { get; set; }
        public AccessToken AccessToken { get; set; } = null!;
    }
}