namespace NDE_Digital_Market.Services.UserControllOfDashboardService
{
    public interface IUserControllOfDashboard_Service
    {
        Task<object> DeleteMenuItems(string UserId, List<string> menuIdsToDelete);
    }
}
