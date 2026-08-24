namespace FocusSpace.Requests
{
    public class CreateProductRequest
    {
        public required string Name { get; set; }
        public required string SKU { get; set; }
        public required string Category { get; set; }
        public decimal Price { get; set; }
        public int MinimumStock { get; set; }
    }
}
