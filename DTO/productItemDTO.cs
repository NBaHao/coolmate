namespace CoolMate.DTO
{
    public class ProductItemDTO
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public string? ColorImage { get; set; }
        public int? QtyInStock { get; set; }
        public List<ProductItemImageDTO> ProductItemImages { get; set; } = new List<ProductItemImageDTO>();
    }
}
