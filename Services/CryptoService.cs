using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

public class CryptoService
{
    private readonly string _secret = "CHANGE_THIS_SECRET_KEY_32+_CHARS";

    public string CreateTicket(int userId, string email)
    {
        var payload = new
        {
            uid = userId,
            email = email,
            iat = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            exp = DateTimeOffset.UtcNow.AddHours(8).ToUnixTimeSeconds()
        };

        string json = JsonSerializer.Serialize(payload);
        string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

        string signature = Sign(base64);

        return $"t={base64}&p={signature}";
    }

    public bool ValidateTicket(string ticket)
    {
        var parts = ticket.Split('&');
        if (parts.Length != 2) return false;

        var t = parts[0].Replace("t=", "");
        var p = parts[1].Replace("p=", "");

        return Sign(t) == p;
    }

    private string Sign(string data)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secret));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        return Convert.ToBase64String(hash);
    }
}