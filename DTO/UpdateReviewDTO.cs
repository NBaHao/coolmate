namespace CoolMate.DTO
{
    public class UpdateReviewDTO
    {
        public int Id { get; set; }
        public int? RatingValue { get; set; }
        public string Comment { get; set; }
    }
}