namespace CoolMate.DTO
{
    public class CreateOrderDTO
    {
        public string shippingAddress { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public int paymentMethod { get; set; }
        public int shippingMethod { get; set; }
    }
}