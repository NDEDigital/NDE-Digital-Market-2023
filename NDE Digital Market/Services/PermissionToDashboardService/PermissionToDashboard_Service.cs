using NDE_Digital_Market.Data_Access_Layer;
using NDE_Digital_Market.Model.DTO;
using NDE_Digital_Market.SharedServices;
using System.Data;

namespace NDE_Digital_Market.Services.PermissionToDashboardService
{
    public class PermissionToDashboard_Service : IPermissionToDashboard_Service
    {

        private readonly PermissionToDashboard_DAL _permissionToDashboard_DAL;
        public PermissionToDashboard_Service(PermissionToDashboard_DAL permissionToDashboard_DAL)
        {
             _permissionToDashboard_DAL =  permissionToDashboard_DAL;
        }


        public async Task<object> InsertPermissionToDashboard(string UserId, string MenuId)
        {
            int decryptedUserId = int.Parse(CommonServices.DecryptPassword(UserId));
            int decryptedMenuId = int.Parse(CommonServices.DecryptPassword(MenuId));

            return await _permissionToDashboard_DAL.InsertPermissionToDashboard(decryptedUserId, decryptedMenuId);

        }


        public async Task<Dictionary<string, List<GetPermissionToDashBoardListDTO>>> GetPermissionData(string UserId)
        {
            int decryptedUserId = int.Parse(CommonServices.DecryptPassword(UserId));
            DataTable dataTable = await _permissionToDashboard_DAL.GetPermissionData(decryptedUserId);

            var result = new Dictionary<string, List<GetPermissionToDashBoardListDTO>>();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }
            foreach (DataRow row in dataTable.Rows)
            {
                var userId = CommonServices.EncryptPassword(row["UserId"].ToString());

                var permission = new GetPermissionToDashBoardListDTO
                {
                    UserId = userId,
                    MenuId = CommonServices.EncryptPassword(row["MenuId"].ToString()),
                    MenuName = row["MenuName"].ToString(),
                    FullName = row["FullName"].ToString(),
                    PermissionId = CommonServices.EncryptPassword(row["PermissionId"].ToString()),
                };

                if (!result.ContainsKey(userId))
                {
                    result[userId] = new List<GetPermissionToDashBoardListDTO>();
                }

                result[userId].Add(permission);
            }
            return result;

        }


    }
}
