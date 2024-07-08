namespace NDE_Digital_Market.Model.DTO
{
    public class GetProductReviewAndRatingByProductIdDTO
    {

        public string? BuyerName { get; set; }

        public string? CompanyName { get; set; }

        public string? EmptyRatingArray { get; set; }
        public string? RatingArray { get; set; }
        public string? ReviewId { get; set; }

        public string? OrderDetailId { get; set; }

        public string? ReviewText { get; set; }

        public int? RatingValue { get; set; }

        public string? BuyerId { get; set; }




        public string? CompanyCode { get; set; }

        public DateTime? ReviewDate { get; set; }

        public string? ImagePath { get; set; }
    }
}
