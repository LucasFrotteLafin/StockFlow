namespace FocusSpace.Models
{
    public class UserRequest
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public string Status { get; set; } = "Pendente";
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;
        public DateTime? ApprovedDate { get; set; }
    }
}
