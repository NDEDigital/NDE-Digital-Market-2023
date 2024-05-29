using NDE_Digital_Market.Model.DTO;

namespace NDE_Digital_Market.Services.CompanyAdminService
{
    public interface ICompanyAdmin_Service
    {
        Task<List<GetCompanySellerListDTO>> CompanySellerDetails(string userId, bool IsActive);



        Task<object> UpdateUserStatus(string userId, bool IsActive);
    }
}
