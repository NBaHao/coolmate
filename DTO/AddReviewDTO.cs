namespace CoolMate.DTO
{
    public class AddReviewDTO
    {
        public int? ProductItemId { get; set; }
        public int? RatingValue { get; set; }
        public string Comment { get; set; }
    }
}