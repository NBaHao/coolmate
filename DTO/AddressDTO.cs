namespace CoolMate.DTO
{
    public class AddressDTO
    {
        public int addressId {  get; set; }
        public string streetLine { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public int? isDefault { get; set; }
    }
}
