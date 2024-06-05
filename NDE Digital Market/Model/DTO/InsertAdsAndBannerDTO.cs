namespace NDE_Digital_Market.Model.DTO
{
    public class InsertAdsAndBannerDTO
    {
        public string? UserId { get; set; }
        public bool? IsActive { get; set; }

        public bool? IsAds { get; set; }
        public string? AddedBy { get; set; }
        public string? AddedPC { get; set; }
        public string? CompanyCode { get; set; }
        public string? BannerDescription { get; set; }
        public IFormFile? BannerImageFile { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsPayment { get; set; }
        public string? PaymentRemarks { get; set; }
        public bool? IsBannerStatus { get; set; }
    }
}
