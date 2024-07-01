
using NDE_Digital_Market.Data_Access_Layer;
using NDE_Digital_Market.Model.DTO;
using NDE_Digital_Market.SharedServices;
using System.Data;

namespace NDE_Digital_Market.Services.SellerInventoryService;

public class SellerInventory_Service : ISellerInventory_Service
{
    private readonly SellerInventory_DAL _sellerInventory_DAL;
    public SellerInventory_Service(SellerInventory_DAL sellerInventory_DAL)
    {
        _sellerInventory_DAL = sellerInventory_DAL;
    }

    public async Task<List<GetSellerInventoryInfoDTO>> GetSellerInventoryDataBySellerId(string UserId)
    {
        try
        {
            string decryptedId = CommonServices.DecryptPassword(UserId);
            int decryptedIdInt;
            int.TryParse(decryptedId, out decryptedIdInt);
            DataTable dataTable =  await _sellerInventory_DAL.GetSellerInventoryDataBySellerId(decryptedIdInt);

            List<GetSellerInventoryInfoDTO> unitlist = new List<GetSellerInventoryInfoDTO>();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }
            foreach (DataRow row in dataTable.Rows)
            {
                GetSellerInventoryInfoDTO modelObj = new GetSellerInventoryInfoDTO
                {

                    ProductName = row["ProductName"].ToString(),
                    ProductGroupName = row["ProductGroupName"].ToString(),
                    Specification = row["Specification"].ToString(),
                    Price = row["Price"] != DBNull.Value ? Convert.ToDecimal(row["Price"]) : 0,
                    Unit = row["Unit"].ToString(),
                    TotalQty = Convert.ToInt32(row["TotalQty"]),
                    AvailableQty = Convert.ToInt32(row["AvailableQty"]),
                    SaleQty = Convert.ToInt32(row["SaleQty"])
                };
                unitlist.Add(modelObj);
            }

                
            return unitlist;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}
    