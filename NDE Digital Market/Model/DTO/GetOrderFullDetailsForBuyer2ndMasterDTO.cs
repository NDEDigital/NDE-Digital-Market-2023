namespace NDE_Digital_Market.Model.DTO
{
    public class GetOrderFullDetailsForBuyer2ndMasterDTO
    {
        public string? SellerId { get; set; }
        public string? SellerName { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string? Status { get; set; }
        public decimal? PackageSubtotal { get; set; } = decimal.Zero;
        public decimal? PackageDeliveryCharge { get; set; } = decimal.Zero;

        public List<GetOrderFullDetailsForBuyer2ndMDetailsDTO>? OrderDetails2ndMDetailsListForBuyer { get; set; }

        public GetOrderFullDetailsForBuyer2ndMasterDTO()
        {
            OrderDetails2ndMDetailsListForBuyer = new List<GetOrderFullDetailsForBuyer2ndMDetailsDTO>();
        }
    }
}
