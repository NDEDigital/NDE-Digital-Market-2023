using NDE_Digital_Market.Data_Access_Layer;
using NDE_Digital_Market.Model;
using NDE_Digital_Market.Model.DTO;
using NDE_Digital_Market.SharedServices;
using System.Data;

namespace NDE_Digital_Market.Services.AddToCartService
{
    public class AddToCart_Service : IAddToCart_Service
    {

        private readonly AddToCart_DAL _AddToCart_DAL;
        public AddToCart_Service(AddToCart_DAL AddToCart_DAL)
        {
            _AddToCart_DAL = AddToCart_DAL;
        }

        public async Task<List<GetAddToCartDataDTO>> GetAddToCartData()
        {
            DataTable dataTable = await _AddToCart_DAL.GetAddToCartData();

            List<GetAddToCartDataDTO> list = new List<GetAddToCartDataDTO>();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                GetAddToCartDataDTO addToCart = new GetAddToCartDataDTO();
                addToCart.Id = CommonServices.EncryptPassword(row["Id"].ToString());
                addToCart.BuyerUserID = CommonServices.EncryptPassword(row["BuyerUserID"].ToString());
                addToCart.CompanyCode = CommonServices.EncryptPassword(row["CompanyCode"].ToString());
                addToCart.ProductID = CommonServices.EncryptPassword(row["ProductID"].ToString());
                addToCart.ProductGroupID = CommonServices.EncryptPassword(row["ProductGroupID"].ToString());
                addToCart.UnitID = CommonServices.EncryptPassword(row["UnitID"].ToString());
                addToCart.ProductCartQuantity = (int)row["ProductCartQuantity"];
                addToCart.AddedDate = row["AddedDate"] as DateTime?;
                addToCart.AddedBy = row["AddedBy"] as string;
                addToCart.AddedPC = row["AddedPC"] as string;
                addToCart.UpdatedDate = row["UpdatedDate"] as DateTime?;
                addToCart.UpdatedBy = row["UpdatedBy"] as string;
                addToCart.UpdatedPC = row["UpdatedPC"] as string;

                list.Add(addToCart);
            }

            return list;
        }



        public async Task<List<GetAddToCartDataDTO>> GetAddToCartDataByUserID(string userId)
        {
            int decryptUserId = int.Parse(CommonServices.EncryptPassword(userId));
            DataTable dataTable = await _AddToCart_DAL.GetAddToCartDataByUserID(decryptUserId);

            List<GetAddToCartDataDTO> list = new List<GetAddToCartDataDTO>();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                GetAddToCartDataDTO addToCart = new GetAddToCartDataDTO();
                addToCart.Id = CommonServices.EncryptPassword(row["Id"].ToString());
                addToCart.BuyerUserID = CommonServices.EncryptPassword(row["BuyerUserID"].ToString());
                addToCart.CompanyCode = CommonServices.EncryptPassword(row["CompanyCode"].ToString());
                addToCart.ProductID = CommonServices.EncryptPassword(row["ProductID"].ToString());
                addToCart.ProductGroupID = CommonServices.EncryptPassword(row["ProductGroupID"].ToString());
                addToCart.UnitID = CommonServices.EncryptPassword(row["UnitID"].ToString());
                addToCart.ProductCartQuantity = (int)row["ProductCartQuantity"];
                addToCart.ImagePath = (string)row["ImagePath"].ToString();
                addToCart.ProductName = (string)row["ProductName"].ToString();
                addToCart.Price = (string)row["Price"].ToString();
                addToCart.TotalPrice = (string)row["TotalPrice"].ToString();
                addToCart.AvailableQty = (string)row["AvailableQty"].ToString();

                addToCart.CompanyName = (string)row["CompanyName"].ToString();
                addToCart.Specification = (string)row["Specification"].ToString();

                list.Add(addToCart);
            }

            return list;
        }



        public async Task<object> AddToCartData(InsertAddToCartDataDTO addToCart)
        {
            AddToCartModal Model = new AddToCartModal();
            Model.BuyerUserID = int.Parse(CommonServices.DecryptPassword(addToCart.BuyerUserID));
            Model.CompanyCode = CommonServices.DecryptPassword(addToCart.BuyerUserID);
            Model.ProductID = int.Parse(CommonServices.DecryptPassword(addToCart.ProductID));
            Model.ProductGroupID = int.Parse(CommonServices.DecryptPassword(addToCart.ProductGroupID));
            Model.UnitID = int.Parse(CommonServices.DecryptPassword(addToCart.UnitID));
            Model.ProductCartQuantity = addToCart.ProductCartQuantity;
            Model.AddedDate = DateTime.UtcNow;
            Model.AddedBy = addToCart.AddedBy;
            Model.AddedPC = addToCart.AddedPC;
            Model.ImagePath = addToCart.ImagePath;
            Model.ProductName = addToCart.ProductName;
            Model.Price = addToCart.Price;
            Model.TotalPrice = addToCart.TotalPrice;
            Model.AvailableQty = addToCart.AvailableQty;
            Model.CompanyName = addToCart.CompanyName;
            Model.Specification = addToCart.Specification;


            return await _AddToCart_DAL.AddToCartData(Model);
        }



        public async Task<object> DeleteAddToCart(string id)
        {
            int decryptUserId = int.Parse(CommonServices.EncryptPassword(id));
            return await _AddToCart_DAL.GetAddToCartDataByUserID(decryptUserId);
        }

    }
}
