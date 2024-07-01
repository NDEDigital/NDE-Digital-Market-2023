using NDE_Digital_Market.Data_Access_Layer;
using NDE_Digital_Market.Model.DTO;
using NDE_Digital_Market.SharedServices;
using System.Data;

namespace NDE_Digital_Market.Services.WishListService
{
    public class WishList_Service : IWishList_Service
    {
        private readonly WishList_DAL _wishList_DAL;
        public WishList_Service(WishList_DAL wishList)
        {
            _wishList_DAL = wishList;
        }

        public async Task<List<GetAllWishListDTO>> GetWishList(string UserId)
        {

            int decryptedUserId = int.Parse(CommonServices.DecryptPassword(UserId));

            DataTable dataTable = await _wishList_DAL.GetWishList(decryptedUserId);

            List<GetAllWishListDTO> wishList = new List<GetAllWishListDTO>();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                GetAllWishListDTO wishListItem = new GetAllWishListDTO();

                //wishListItem.UserId = CommonServices.Encrypt(Convert.ToInt32(row["UserId"]));
                wishListItem.CompanyCode = CommonServices.EncryptPassword(row["CompanyCode"].ToString());
                wishListItem.CompanyName = row["CompanyName"].ToString();
                wishListItem.ProductGroupName = row["ProductGroupName"].ToString();
                wishListItem.ProductId = CommonServices.EncryptPassword(row["ProductId"].ToString());
                wishListItem.ProductName = row["ProductName"].ToString();
                wishListItem.GroupCode = CommonServices.EncryptPassword(row["ProductGroupCode"].ToString());
                wishListItem.SellerId = CommonServices.EncryptPassword(row["SellerId"].ToString());
                wishListItem.ProductGroupID = CommonServices.EncryptPassword((row["ProductGroupID"].ToString()));
                wishListItem.Specification = row["Specification"].ToString();
                wishListItem.UnitId = CommonServices.EncryptPassword(row["UnitId"].ToString());
                wishListItem.Unit = row["Unit"].ToString();
                wishListItem.Price = row["Price"] != DBNull.Value ? Convert.ToDecimal(row["Price"]) : 0;
                wishListItem.DiscountAmount = row["DiscountAmount"] != DBNull.Value ? Convert.ToDecimal(row["DiscountAmount"]) : 0;
                wishListItem.DiscountPct = row["DiscountPct"] != DBNull.Value ? Convert.ToDecimal(row["DiscountPct"]) : 0;
                wishListItem.ImagePath = row["ImagePath"].ToString();
                wishListItem.TotalPrice = row["TotalPrice"] != DBNull.Value ? Convert.ToDecimal(row["TotalPrice"]) : 0;

                wishListItem.AvailableQty = Convert.ToInt32(row["AvailableQty"]);
                DateTime? endDate = null;
                if (row["EndDate"] != DBNull.Value)
                {
                    endDate = Convert.ToDateTime(row["EndDate"]);
                    if (endDate <= DateTime.Now)
                    {

                        wishListItem.TotalPrice = wishListItem.Price;
                        wishListItem.DiscountAmount = 0;
                        wishListItem.DiscountPct = 0;
                    }
                }

                wishList.Add(wishListItem);
            }

            return wishList;

        }
        public async Task<object> InsertWishList(string UserId, string ProductId, string CompanyCode)
        {
            int decryptedUserId = int.Parse(CommonServices.DecryptPassword(UserId));
            int decryptedProductId = int.Parse(CommonServices.DecryptPassword(ProductId));
            string decryptedCompanyCode = CommonServices.DecryptPassword(CompanyCode);
            return await _wishList_DAL.InsertWishList(decryptedUserId, decryptedProductId, decryptedCompanyCode);
        }

        public async Task<object> DeleteWishList(string UserId, string ProductId, string CompanyCode)
        {
            int decryptedUserId = int.Parse(CommonServices.DecryptPassword(UserId));
            int decryptedProductId = int.Parse(CommonServices.DecryptPassword(ProductId));
            string decryptedCompanyCode = CommonServices.DecryptPassword(CompanyCode);

            return await _wishList_DAL.DeleteWishList(decryptedUserId, decryptedProductId, decryptedCompanyCode);
        }
    }
}
