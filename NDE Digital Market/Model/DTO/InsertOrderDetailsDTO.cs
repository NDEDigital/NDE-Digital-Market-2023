namespace NDE_Digital_Market.Model.DTO
{
    public class InsertOrderDetailsDTO
    {
        public string CompanyCode { get; set; }
        public string? OrderMasterId { get; set; }
        public string? UserId { get; set; }
        public string? ProductId { get; set; }
        public string? ProductGroupID { get; set; }
        public string? Specification { get; set; }
        public int? Qty { get; set; }
        public string? UnitId { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? Price { get; set; }
        public decimal? DeliveryCharge { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public decimal? DiscountPct { get; set; }
        public decimal? NetPrice { get; set; }
        public string? AddedBy { get; set; }
        public string? AddedPC { get; set; }
    }
}
