namespace NDE_Digital_Market.Model
{
    public class RecommendedProductListModel
    {
        public int? ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ImagePath { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? CompanyCode { get; set; }
        public decimal AvailableQty { get; set; }
    }
}
