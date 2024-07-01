using NDE_Digital_Market.Data_Access_Layer;
using NDE_Digital_Market.Model.DTO;
using NDE_Digital_Market.SharedServices;
using System.Data;

namespace NDE_Digital_Market.Services.DashboardService
{
    public class Dashboard_Service : IDashboard_Service
    {

        private readonly Dashboard_DAL _dashboard_DAL;
        public Dashboard_Service(Dashboard_DAL dashboard_DAL)
        {
            _dashboard_DAL = dashboard_DAL;
        }
        public async Task<List<GetSellerDashBoardMenuListDTO>> CompanySellerDetails(string UserId)
        {
            int decryptuserid = int.Parse(CommonServices.DecryptPassword(UserId));
            DataTable dataTable = await _dashboard_DAL.CompanySellerDetails(decryptuserid);

            List<GetSellerDashBoardMenuListDTO> list = new List<GetSellerDashBoardMenuListDTO>();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                GetSellerDashBoardMenuListDTO obj = new GetSellerDashBoardMenuListDTO();
                obj.MenuId = CommonServices.EncryptPassword(row["MenuId"].ToString());
                obj.MenuName = row["MenuName"].ToString();
                list.Add(obj);
            }

            return list;
        }
    }
}
