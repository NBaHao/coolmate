namespace CoolMate.DTO
{
    public class CreateOrderDTO
    {
        public string shippingAddress { get; set; }
        public int paymentMethod { get; set; }
        public int shippingMethod { get; set; }
    }
}