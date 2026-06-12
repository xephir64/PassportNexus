using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;

public class CryptoService
{
    private readonly string _secret = "azertyuiopqsdfghjklmwxcvbn1234567890&é'(-è_çà)"; // change this for a more robust secret.

    public PassportTicket CreateTicket(int userId, string userEmail)
    {
        var payload = new
        {
            uid = userId,
            email = userEmail,
            iat = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            exp = DateTimeOffset.UtcNow.AddHours(8).ToUnixTimeSeconds()
        };

        string json = JsonSerializer.Serialize(payload);
        string base64 = Base64UrlEncoder.Encode(Encoding.UTF8.GetBytes(json));
        
        string signature = Sign(base64);

       string ticket = $"t={base64}&p={signature}";
       return new PassportTicket(ticket, payload.exp, userId, userEmail);
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
        return Base64UrlEncoder.Encode(hash);
    }
}