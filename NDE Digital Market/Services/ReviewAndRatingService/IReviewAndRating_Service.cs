using NDE_Digital_Market.Model;

namespace NDE_Digital_Market.Services.ReviewAndRatingService
{
    public interface IReviewAndRating_Service
    {
        Task<object> getReviewRatingsData(string ProductId);


        Task<object> AddReview(ReviewsAndRatings review);
    }
}
