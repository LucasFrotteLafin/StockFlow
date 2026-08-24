namespace FocusSpace.Requests
{
    public class CreateMovementRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public required string Type { get; set; }
        public required string Reason { get; set; }
        public int UserId { get; set; }
    }
}
