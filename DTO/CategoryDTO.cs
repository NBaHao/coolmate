namespace CoolMate.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string slug { get; set; }
        public List<CategoryDTO> Children { get; set; }
    }
}
