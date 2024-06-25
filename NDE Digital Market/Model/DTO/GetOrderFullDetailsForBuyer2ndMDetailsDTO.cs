namespace NDE_Digital_Market.Model.DTO
{
    public class GetOrderFullDetailsForBuyer2ndMDetailsDTO
    {
        public string ProductName { get; set; }
        public string? Imagepath { get; set; }
        public decimal? Price { get; set; }
        public decimal? ProductTotalPrice { get; set; }
        public decimal? ProductSubtotal { get; set; }
        public int? TotalQty { get; set; }
        public decimal? DeliveryCharge { get; set; }
    }
}
