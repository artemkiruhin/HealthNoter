using System.Security.Cryptography;
using System.Text;

namespace HealthNoter.Service.Hash;

public class Sha256HashService : IHashService
{
    public string ComputeHash(string message)
    {
        var encodedMessage = Encoding.UTF8.GetBytes(message);
        var hexHash = SHA256.HashData(encodedMessage);
        var hash = Convert.ToHexStringLower(hexHash);

        return hash;
    }
}