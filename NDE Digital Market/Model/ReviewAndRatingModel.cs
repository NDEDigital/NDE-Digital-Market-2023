namespace NDE_Digital_Market.Model
{
    public class ReviewAndRatingModel
    {
        public IFormFile? ImageFile { get; set; }
        public string? AddedBy { get; set; }
        public string? AddedPc { get; set; }
        public int? BuyerId { get; set; }
        public int? OrderDetailId { get; set; }
        public string? ReviewText { get; set; }

        public int? RatingValue { get; set; }
        public string? ImagePath { get; set; }

        public int? ProductId { get; set; }

        public int? SellerId { get; set; }
    }
}
