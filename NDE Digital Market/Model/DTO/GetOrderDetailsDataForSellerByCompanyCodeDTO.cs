namespace NDE_Digital_Market.Model.DTO
{
    public class GetOrderDetailsDataForSellerByCompanyCodeDTO
    {
        public string? OrderDetailId { get; set; }
        public string? ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ImagePath { get; set; }
        public int? Qty { get; set; }
        public decimal? Price { get; set; }
        public decimal? DeliveryCharge { get; set; }
        public string? Specification { get; set; }
        public decimal? StockQty { get; set; }
        public int? SaleQty { get; set; }
        public string? UnitId { get; set; }
        public decimal? NetPrice { get; set; }
        public string? ProductGroupID { get; set; }

        public string? Status { get; set; }
        public string? ReturnTypeName { get; set; }
    }
}
