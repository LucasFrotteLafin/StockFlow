namespace FocusSpace.Models
{
    public class Movement
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public required string Type { get; set; }
        public required string Reason { get; set; }
        public DateTime MovementDate { get; set; }
        public int UserId { get; set; }
        public required string UserName { get; set; }
    }
}
