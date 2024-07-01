using NDE_Digital_Market.Model;
using NDE_Digital_Market.Model.DTO;

namespace NDE_Digital_Market.Services.AddToCartService
{
    public interface IAddToCart_Service
    {
        Task<List<GetAddToCartDataDTO>> GetAddToCartData();



        Task<List<GetAddToCartDataDTO>> GetAddToCartDataByUserID(string userId);



        Task<object> AddToCartData(InsertAddToCartDataDTO addToCart);



        Task<object> DeleteAddToCart(string id);
    }
}
