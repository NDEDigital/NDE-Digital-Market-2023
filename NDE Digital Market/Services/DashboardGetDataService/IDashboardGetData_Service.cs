using NDE_Digital_Market.Model.DTO;

namespace NDE_Digital_Market.Services.DashboardGetDataService
{
    public interface IDashboardGetData_Service
    {
        Task<List<GetPermissionToDashBoardDataDTO>> GetPermissionData(string UserId, int Status1);

    }
}
