namespace NDE_Digital_Market.Model.DTO
{
    public class GetOrderMasterDataForBuyerByUserIdDTO
    {
        public string? OrderMasterId { get; set; }
        public string? OrderNo { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal? TotalPrice { get; set; }



        public List<GetOrderDetailsDataForBuyerByUserIdDTO>? OrderDetailsListForBuyer { get; set; }


        public GetOrderMasterDataForBuyerByUserIdDTO()
        {
            OrderDetailsListForBuyer = new List<GetOrderDetailsDataForBuyerByUserIdDTO>();
        }
    }
}
