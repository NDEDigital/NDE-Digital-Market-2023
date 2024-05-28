namespace NDE_Digital_Market.Model.DTO
{
    public class GetOrderMasterDataForSellerByCompanyCodeDTO
    {
        public string? OrderMasterId { get; set; }
        public string? OrderNo { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal? TotalPrice { get; set; } = decimal.Zero;
        public decimal? TotalDeliveryCharge { get; set; } = decimal.Zero;
        public decimal? TotalAmount { get; set; } = decimal.Zero;
        public string? SellerUserId { get; set; }
        public string? BuyerUserId { get; set; }
        public string? BuyerAddress { get; set; }


        public List<GetOrderDetailsDataForSellerByCompanyCodeDTO>? OrderDetailsListForSeller { get; set; }


        public GetOrderMasterDataForSellerByCompanyCodeDTO()
        {
            OrderDetailsListForSeller = new List<GetOrderDetailsDataForSellerByCompanyCodeDTO>();
        }
    }
}
