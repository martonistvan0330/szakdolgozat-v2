namespace HomeworkManager.Model.Entities
{
    public class RefreshToken
    {
        public int RefreshTokenId { get; set; }
        public required string Token { get; set; }
        public Guid UserId { get; set; }
    }
}