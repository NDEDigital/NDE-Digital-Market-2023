using NDE_Digital_Market.Data_Access_Layer;
using NDE_Digital_Market.SharedServices;

namespace NDE_Digital_Market.Services.UserControllOfDashboardService
{
    public class UserControllOfDashboard_Service : IUserControllOfDashboard_Service
    {

        private readonly UserControllOfDashboard_DAL _UserControllOfDashboard_DAL;
        public UserControllOfDashboard_Service(UserControllOfDashboard_DAL UserControllOfDashboard_DAL)
        {
            _UserControllOfDashboard_DAL = UserControllOfDashboard_DAL;
        }

        public async Task<object> DeleteMenuItems(string UserId, List<string> menuIdsToDelete)
        {

            List<int> demenuIdsToDelete = new List<int>();

            for (int i = 0; i < menuIdsToDelete.Count; i++)
            {
                demenuIdsToDelete[i] = int.Parse(CommonServices.DecryptPassword(menuIdsToDelete[i]));
            }
            int decryptUserId = int.Parse(CommonServices.DecryptPassword(UserId));
            return await _UserControllOfDashboard_DAL.DeleteMenuItems(decryptUserId, demenuIdsToDelete);
        }
    }
}
