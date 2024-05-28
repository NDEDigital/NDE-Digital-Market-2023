namespace NDE_Digital_Market.Model.DTO
{
    public class UpdateOrderDTO
    {
        public string? orderdetailsIds { get; set; }
        public string? status { get; set; }
        public SellerSalesMasterModel? sellerSalesMasterModel { get; set; }
    }
}
