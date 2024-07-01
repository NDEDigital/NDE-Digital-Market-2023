namespace NDE_Digital_Market.Model.DTO
{
    public class GetSellerOrderDataByUserIdAndStatusDTO
    {
        public string OrderDetailId { get; set; }
        public string OrderMasterId { get; set; }
        public string? OrderNo { get; set; }
        public string? Address { get; set; }
        public string? BUserId { get; set; }
        public string? BuyerName { get; set; }
        public string? ProductGroupID { get; set; }
        public string? ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? Specification { get; set; }
        public decimal? StockQty { get; set; }
        public decimal? TotalQty { get; set; } = 0;
        public int? SaleQty { get; set; }
        public string? UnitId { get; set; }
        public string? Unit { get; set; }
        public decimal? NetPrice { get; set; }
        public string? Status { get; set; }
        public string? ReturnTypeName { get; set; }
    }
}
