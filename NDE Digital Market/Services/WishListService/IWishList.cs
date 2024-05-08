using NDE_Digital_Market.DTOs;

namespace NDE_Digital_Market.Services.WishListService
{
    public interface IWishList
    {
        Task<List<WishListDTO>> GetWishList(int UserId);

        Task<object> InsertWishList(int UserId, string ProductId, string CompanyCode);

        Task<object> DeleteWishList(int UserId, string ProductId, string CompanyCode);
    }
}
