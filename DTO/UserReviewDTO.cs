namespace CoolMate.DTO
{
    public class UserReviewDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public int? OrderedProductId { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public int? RatingValue { get; set; }
        public string Comment { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
    }
}