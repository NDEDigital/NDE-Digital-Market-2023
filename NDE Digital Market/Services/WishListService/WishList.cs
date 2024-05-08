using NDE_Digital_Market.Data_Access_Layer;
using NDE_Digital_Market.DTOs;

namespace NDE_Digital_Market.Services.WishListService
{
    public class WishList : IWishList
    {
        private readonly WishList_DAL _wishList_DAL;
        public WishList(WishList_DAL wishList)
        {
            _wishList_DAL = wishList;
        }

        public async Task<List<WishListDTO>> GetWishList(int UserId)
        {

            return await _wishList_DAL.GetWishList(UserId);
        }
        public async Task<object> InsertWishList(int UserId, string ProductId, string CompanyCode)
        {
            return await _wishList_DAL.InsertWishList(UserId, ProductId, CompanyCode);
        }

        public async Task<object> DeleteWishList(int UserId, string ProductId, string CompanyCode)
        {
            return await _wishList_DAL.DeleteWishList(UserId, ProductId, CompanyCode);
        }
    }
}
