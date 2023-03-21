using System.Security.Cryptography;
using System.Text;

namespace partsbin.Logic.Helpers;

public static class StringExtensions
{
    public static string GetSha256(this string input)
    {
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = SHA256.HashData(inputBytes);

        var sb = new StringBuilder();
        foreach (var b in hashBytes)
        {
            sb.Append(b.ToString("x2"));
        }

        var output = sb.ToString();

        return output;
    }
}