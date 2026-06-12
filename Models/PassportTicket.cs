public class PassportTicket
{
    public string Ticket { get; set; } = "";
    public DateTime Expiration { get; set; }
    public int UserId { get; set; }
    public string Email { get; set; } = "";
}