using NDE_Digital_Market.Data_Access_Layer;
using NDE_Digital_Market.Model.DTO;
using NDE_Digital_Market.SharedServices;
using System.Data;

namespace NDE_Digital_Market.Services.DashboardGetDataService
{
    public class DashboardGetData_Service : IDashboardGetData_Service
    {
        private readonly DashboardGetData_DAL _dashboardGetData_DAL;

        public DashboardGetData_Service(DashboardGetData_DAL dashboardGetData_DAL)
        {
            _dashboardGetData_DAL = dashboardGetData_DAL;
        }
        public async Task<List<GetPermissionToDashBoardDataDTO>> GetPermissionData(string UserId, int Status1)
        {
            int DecryptUserId = int.Parse(CommonServices.DecryptPassword(UserId));
            DataTable dataTable = await _dashboardGetData_DAL.GetPermissionData(DecryptUserId, Status1);

            var result = new List<GetPermissionToDashBoardDataDTO>();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                var permission = new GetPermissionToDashBoardDataDTO();
                string userid = row["UserId"].ToString();
                if(userid != null)
                {
                    permission.UserId = CommonServices.EncryptPassword(userid);
                }
                
                permission.MenuId = CommonServices.EncryptPassword(row["MenuId"].ToString());
                permission.MenuName = row["MenuName"].ToString();
                // Add other properties if needed
                result.Add(permission);
            }
            return result;
        }

    }
}
