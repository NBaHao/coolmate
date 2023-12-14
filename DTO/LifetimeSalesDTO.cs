namespace CoolMate.DTO
{
    public class LifetimeSalesDTO
    {
        public int TotalOrders { get; set; }
        public long TotalSales { get; set; }
        public float CompletedPercentage { get; set; }
        public float CancelledPercentage { get; set;}
    }
}
