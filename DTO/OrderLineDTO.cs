namespace CoolMate.DTO
{
    public class OrderLineDTO
    {
        public int Id { get; set; }
        public int? ProductItemId { get; set; }
        public string? ProductName { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public int? Quantity { get; set; }
        public int? Price { get; set; }
        public string? Img { get; set; }
    }
}