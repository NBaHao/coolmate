namespace CoolMate.DTO
{
    public class CategoryTreeDTO
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public List<CategoryTreeDTO> Children { get; set; }
        public string Slug { get; set; }
    }
}
