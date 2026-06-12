using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("rdr")]
public class NexusController : ControllerBase
{
    [HttpGet("pprdr.asp")]
    public IActionResult Get()
    {
        Console.WriteLine("Hello Nexus");
        Response.Headers["Cache-Control"] = "private";
        Response.Headers["Content-Type"] = "text/html";
        Response.Headers["Content-Length"] = "0";
        Response.Headers["PassportURLs"] =
            "DARealm=Passport.Net," +
            "DALogin=login.passport.com/login2.srf," +
            "ConfigVersion=15";

        return Content("", "text/html");
    }
}