public class PassportTicket
{
    public PassportTicket(string ticket, long exp, int userId, string userEmail)
    {
        Ticket = ticket;
        Expiration = exp;
        UserId = userId;
        Email = userEmail;
    }
    public string Ticket { get; set; } = "";
    public long Expiration { get; set; }
    public int UserId { get; set; }
    public string Email { get; set; } = "";
}