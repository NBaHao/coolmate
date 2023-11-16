namespace CoolMate.DTO
{
    public class CreateProductItemDTO
    {
        public List<string> Size { get; set; }
        public ColorDTO Color { get; set; }
        public int Qty { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
