using Microsoft.AspNetCore.Mvc;

[ApiController]
public class LoginController : ControllerBase
{
    private readonly UserService _users;
    private readonly CryptoService _crypto;

    public LoginController(UserService users, CryptoService crypto)
    {
        _users = users;
        _crypto = crypto;
    }

    [HttpGet("/login2.srf")]
    public async Task<IActionResult> Login()
    {
        Console.WriteLine(Request.Method);

        if (Request.Headers.ContainsKey("Authorization"))
        {
            string auth = Request.Headers["Authorization"];

            string email = Extract(auth, "sign-in=");
            string password = Extract(auth, "pwd=");

            var user = await _users.GetUser(email.Replace("%40", "@"), password);

            if (user == null)
            {
                Response.Headers["Connection"] = "close";
                Response.Headers["Content-Type"] = "text/html";
                Response.Headers["Cache-Control"] = "no-cache";
                Response.Headers["cachecontrol"]= "no-store";
                Response.Headers["WWW-Authenticate"] = "Passport1.4 da-status=failed,srealm=Passport.NET,ts=-3,prompt,cbtxt=Account%20or%20password%20is%20incorrect.,cburl=http://www.passportimages.com/XPPassportLogo.gif,cbtxt=Type%20your%20e-mail%20address%20and%20password%20correctly.%20If%20you%20haven%E2%80%99t%20registered%20with%20.NET%20Passport%2C%20click%20the%20Get%20a%20.NET%20Passport%20link.";
                return Unauthorized("HTTP/1.1 401 Unauthorized");
            }

            string ticket = _crypto.CreateTicket(user.Id, user.Email);
            Response.Headers["Cache-Control"] = "no-cache";
            Response.Headers["cachecontrol"]= "no-store";
            Response.Headers["Connection"] = "close";
            Response.Headers["Content-Type"] = "text/html";
            Response.Headers["Authentication-Info"] = $"Passport1.4 da-status=success,from-PP='{ticket}',ru=http://messenger.msn.com";
            Response.Headers["Content-Length"] = "0";
            return Content("");

        }
        Response.Headers["Connection"] = "close";
        Response.Headers["Content-Type"] = "text/html";
        Response.Headers["Cache-Control"] = "no-cache";
        Response.Headers["cachecontrol"]= "no-store";
        Response.Headers["WWW-Authenticate"] = "Passport1.4 da-status=failed,cbtxt=There%20were%20an%20error%20with%20the%20.NET%20Passport%20Service%20Please%20try%20again.";
        return Unauthorized("HTTP/1.1 401 Unauthorized");
    }

    private string Extract(string input, string key)
    {
        int i = input.IndexOf(key);
        if (i == -1) return "";

        int start = i + key.Length;
        int end = input.IndexOf(',', start);
        if (end == -1) end = input.Length;

        return input[start..end].Trim();
    }
}