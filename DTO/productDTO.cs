namespace WebApplication1.DTO
{
    public class productDTO
    {
        public int Id { get; set; }

        public int? CategoryId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Img { get; set; }

        public string? Hover { get; set; }

        public int? PriceInt { get; set; }

        public string? PriceStr { get; set; }
    }
}
