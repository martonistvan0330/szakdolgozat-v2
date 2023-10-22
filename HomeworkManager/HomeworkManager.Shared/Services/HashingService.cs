using System.Security.Cryptography;
using System.Text;
using HomeworkManager.Shared.Services.Interfaces;

namespace HomeworkManager.Shared.Services;

public class HashingService : IHashingService
{
    public async Task<string> GetHashString(string input)
    {
        using var algorithm = SHA256.Create();

        var stream = new MemoryStream(Encoding.UTF8.GetBytes(input));

        var hash = await algorithm.ComputeHashAsync(stream);

        return BitConverter.ToString(hash).Replace("-", "");
    }
}