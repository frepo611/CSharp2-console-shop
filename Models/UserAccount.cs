namespace ConsolesShoppen.Models
{
    public class UserAccount
    {
        public int Id { get; set; }
        public required string AccountEmail { get; set; }
        public required string PasswordHash { get; set; }
        public int CustomerId { get; set; }
        public bool IsAdmin { get; set; }
    }
}