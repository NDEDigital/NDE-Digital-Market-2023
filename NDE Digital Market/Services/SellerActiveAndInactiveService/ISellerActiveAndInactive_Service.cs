using NDE_Digital_Market.Model.DTO;

namespace NDE_Digital_Market.Services.SellerActiveAndInactiveService
{
    public interface ISellerActiveAndInactive_Service
    {
        Task<List<GetCompanySellerDetailsDTO>> CompanySellerDetails(string? CompanyCode, bool IsSeller, bool IsActive);

        Task<object> UpdateSellerProductStatusAsync(string userIds, bool isActive);

    }
}
