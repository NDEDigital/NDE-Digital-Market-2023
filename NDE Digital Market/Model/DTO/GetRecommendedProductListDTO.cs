namespace NDE_Digital_Market.Model.DTO
{
    public class GetRecommendedProductListDTO
    {
        public string? ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ImagePath { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? CompanyCode { get; set; }
        public string? CompanyName { get; set; }
        public decimal AvailableQty { get; set; }
    }
}
