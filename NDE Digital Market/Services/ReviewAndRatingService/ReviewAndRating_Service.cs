using NDE_Digital_Market.Data_Access_Layer;
using NDE_Digital_Market.Model;
using NDE_Digital_Market.SharedServices;

namespace NDE_Digital_Market.Services.ReviewAndRatingService;

public class ReviewAndRating_Service : IReviewAndRating_Service
{


    private readonly ReviewAndRating_DAL _ReviewAndRating_DAL;
    public ReviewAndRating_Service(ReviewAndRating_DAL ReviewAndRating_DAL)
    {
        _ReviewAndRating_DAL = ReviewAndRating_DAL;
    }
    public async Task<object> getReviewRatingsData(string ProductId)
    {
        int decryptProductId = int.Parse(CommonServices.DecryptPassword(ProductId));
        return await _ReviewAndRating_DAL.getReviewRatingsData(decryptProductId);
    }


    public async Task<object> AddReview(ReviewsAndRatings review)
    {

        return await _ReviewAndRating_DAL.AddReview(review);
    }


}
