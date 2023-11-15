namespace CoolMate.DTO
{
    public class ShoppingCartDTO
    {
        public int? ProductItemId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string Img { get; set; }
        public int Qty { get; set; }
        public List<ProductItemDetailsDTO> AllItemOfProduct { get; set; }
    }
}
