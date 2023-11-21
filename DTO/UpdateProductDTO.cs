namespace CoolMate.DTO
{
    public class UpdateProductDTO
    {
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? PriceInt { get; set; }
        public string? PriceStr { get; set; }
    }
}
