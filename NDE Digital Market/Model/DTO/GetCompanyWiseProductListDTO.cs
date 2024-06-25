namespace NDE_Digital_Market.Model.DTO
{
    public class GetCompanyWiseProductListDTO
    {
        public string? CompanyCode { get; set; }
        public string? CompanyName { get; set; }
        public string? ProductGroupName { get; set; }
        public string? ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductGroupID { get; set; }
        public string? Specification { get; set; }
        public string? UnitId { get; set; }
        public string? Unit { get; set; }
        public decimal? Price { get; set; } = 0;
        public decimal? DiscountAmount { get; set; } = 0;
        public decimal? DiscountPct { get; set; } = 0;
        public string? ImagePath { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? SellerId { get; set; }
        public int? AvailableQty { get; set; }
    }
}
