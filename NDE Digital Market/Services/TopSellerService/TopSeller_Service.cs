using NDE_Digital_Market.Data_Access_Layer;
using NDE_Digital_Market.Model.DTO;
using NDE_Digital_Market.SharedServices;
using System.Data;

namespace NDE_Digital_Market.Services.TopSellerService
{
    public class TopSeller_Service : ITopSeller_Service
    {
        private readonly TopSeller_DAL _topSeller_DAL;
        public TopSeller_Service(TopSeller_DAL topSeller_DAL)
        {
            _topSeller_DAL = topSeller_DAL;
        }

        public async Task<List<TopSellerListDTO>> getForDropDown()
        {
            DataTable dataTable = await _topSeller_DAL.getForDropDown();

            List<TopSellerListDTO> topsellerlist = new List<TopSellerListDTO>();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }
            foreach (DataRow row in dataTable.Rows)
            {
                TopSellerListDTO modelObj = new TopSellerListDTO
                {
                    CompanyCode = row["CompanyCode"].ToString(),
                    CompanyName = row["CompanyName"].ToString(),
                    CompanyImage = row["CompanyImage"].ToString(),
                    ProductGroupID = CommonServices.EncryptPassword(row["ProductGroupID"].ToString()),
                    TotalQty = Convert.ToInt32(row["TotalQty"]),
                    ProductGroupCode = row["ProductGroupCode"].ToString(),
                };
                topsellerlist.Add(modelObj);

            }
            return topsellerlist;
        }

    }
}
