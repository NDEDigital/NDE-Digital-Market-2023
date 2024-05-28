using NDE_Digital_Market.Model.DTO;

namespace NDE_Digital_Market.Services.DashboardService
{
    public interface IDashboard_Service
    {
        Task<List<GetSellerDashBoardMenuListDTO>> CompanySellerDetails(string UserId);
    }
}
