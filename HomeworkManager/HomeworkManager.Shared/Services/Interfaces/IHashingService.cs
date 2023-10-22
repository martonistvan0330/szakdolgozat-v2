namespace HomeworkManager.Shared.Services.Interfaces;

public interface IHashingService
{
    Task<string> GetHashString(string input);
}