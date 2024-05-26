using NDE_Digital_Market.Model.DTO;

namespace NDE_Digital_Market.Services.PermissionToDashboardService
{
    public interface IPermissionToDashboard_Service
    {
        Task<object> InsertPermissionToDashboard(string UserId, string MenuId);

        Task<Dictionary<string, List<GetPermissionToDashBoardListDTO>>> GetPermissionData(string UserId);
    }
}
