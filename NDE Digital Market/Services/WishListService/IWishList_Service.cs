using NDE_Digital_Market.DTOs;
using NDE_Digital_Market.Model.DTO;

namespace NDE_Digital_Market.Services.WishListService
{
    public interface IWishList_Service
    {
        Task<List<GetAllWishListDTO>> GetWishList(string UserId);

        Task<object> InsertWishList(string UserId, string ProductId, string CompanyCode);

        Task<object> DeleteWishList(string UserId, string ProductId, string CompanyCode);
    }
}
