using CoolMate.DTO;

namespace CoolMate.DTO
{
    public class CreateProductDTO
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int PriceInt { get; set; }
        public string PriceStr { get; set; }
        public List<CreateProductItemDTO> ProductItems { get; set; }
    }
}
