namespace NDE_Digital_Market.Model.DTO
{
    public class GetOrderDetailsDataForBuyerByUserIdDTO
    {
        public string? OrderDetailId { get; set; }
        public string? ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ImagePath { get; set; }
        public int? Qty { get; set; }
        public decimal? Price { get; set; }
        public string? Status { get; set; }
        public string? CompanyCode { get; set; }
    }
}
