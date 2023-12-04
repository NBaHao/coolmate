using CoolMate.Models;

namespace CoolMate.DTO
{
    public class ProductItemResDTO
    {
        public int Id { get; set; }

        public int? ProductId { get; set; }

        public string? Size { get; set; }
        public List<string> Sizes { get; set; } = new List<string>();
        public List<int> ItemIds { get; set; } = new List<int>();

        public string? Color { get; set; }

        public string? ColorImage { get; set; }

        public int? QtyInStock { get; set; }
        public virtual ICollection<string> ProductItemImages { get; set; } = new List<string>();
    }
}
