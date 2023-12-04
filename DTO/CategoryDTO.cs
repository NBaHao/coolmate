namespace CoolMate.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public int ParentCategoryId { get; set; }
        public string categoryName { get; set; }
        public string slug { get; set; }
    }
}
