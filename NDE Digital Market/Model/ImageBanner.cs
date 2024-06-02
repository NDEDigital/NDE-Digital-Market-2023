namespace NDE_Digital_Market.Model
{
    public class ImageBanner
    {
        //public int? UserId { get; set; }
        public int BannerID { get; set; }
        public string? BannerDescription { get; set; }
        //public IFormFile? BannerImageFile { get; set; }
        public DateTime? AddedDate { get; set; }

        public bool? IsBannerStatus { get; set; }


        public bool? IsAds { get; set; }
        public bool? IsActive { get; set; }

        public string? BannerImage { get; set; }

        public string? CompanyName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }


    }
}
