var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<CryptoService>();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ConfigureHttpsDefaults(https =>
    {
        https.SslProtocols =
            System.Security.Authentication.SslProtocols.Tls
            | System.Security.Authentication.SslProtocols.Tls11
            | System.Security.Authentication.SslProtocols.Tls12
            | System.Security.Authentication.SslProtocols.Ssl3;
    });
});

var app = builder.Build();

app.MapControllers();

app.Run();