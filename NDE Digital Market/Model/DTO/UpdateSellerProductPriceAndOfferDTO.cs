namespace NDE_Digital_Market.Model.DTO
{
    public class UpdateSellerProductPriceAndOfferDTO
    {
        public string ProductId { get; set; }
        public string? UserId { get; set; }
        public decimal? Price { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? DiscountPct { get; set; }
        public DateTime? EffectivateDate { get; set; }
        public DateTime? EndDate { get; set; }
        public IFormFile? ImageFile { get; set; }
        //public string? ImagePath { get; set; }
        //public string? Status { get; set; }
        public decimal? TotalPrice { get; set; }
        //public string? CompanyCode { get; set; }
        //public Boolean? IsActive { get; set; }
        //public DateTime? AddedDate { get; set; } 

        public string? UpdatedPC { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
