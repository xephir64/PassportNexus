using MySqlConnector;

public class UserService
{
    private readonly string _connString;

    public UserService(IConfiguration config)
    {
        _connString = config.GetConnectionString("Default");
    }

    public async Task<User?> GetUser(string email, string password)
    {
        using var conn = new MySqlConnection(_connString);
        await conn.OpenAsync();

        var cmd = new MySqlCommand(
            "SELECT id, email, display_name, password, list_version FROM user WHERE email=@e AND password=@p",
            conn);

        cmd.Parameters.AddWithValue("@e", email);
        cmd.Parameters.AddWithValue("@p", password);

        using var reader = await cmd.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return new User
        {
            Id = reader.GetInt32(0),
            Email = reader.GetString(1),
            DisplayName = reader.IsDBNull(2) ? null : reader.GetString(2),
            Password = reader.GetString(3),
            ListVersion = reader.GetInt32(4)
        };
    }
}