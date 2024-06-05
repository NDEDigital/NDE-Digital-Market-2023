namespace NDE_Digital_Market.Model
{
    public class AdsAndBannerModel : CommonFieldsModel
    {
        public int? BannerID { get; set; }
        public int? UserId { get; set; }
        public bool? IsActive { get; set; }

        public bool? IsAds { get; set; }
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
