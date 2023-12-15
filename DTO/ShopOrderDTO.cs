namespace CoolMate.DTO
{
    public class ShopOrderDTO
    {
        public int Id { get; set; }
        public string? ShippingAddress { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public int? PaymentMethod { get; set; }
        public int? ShippingMethod { get; set; }
        public string? UserId { get; set; }
        public int? OrderTotal { get; set; }
        public DateTime? OrderDate { get; set; }
        public int? OrderStatus { get; set; }
        public IEnumerable<OrderLineDTO> OrderLines { get; set; }
    }
}
