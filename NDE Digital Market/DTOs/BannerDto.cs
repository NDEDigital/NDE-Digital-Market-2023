namespace NDE_Digital_Market.DTOs
{
    public class BannerDto
    {
        public int? BannerID { get; set; }
        public int? UserId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? AddedDate { get; set; }
        public string? AddedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public string? AddedPC { get; set; }
        public string? UpdatedPC { get; set; }
        public string? CompanyCode { get; set; }
        public string? BannerDescription { get; set; }
        public IFormFile? BannerImageFile { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsPayment { get; set; }
        public string? PaymentRemarks { get; set; }

    }
}
