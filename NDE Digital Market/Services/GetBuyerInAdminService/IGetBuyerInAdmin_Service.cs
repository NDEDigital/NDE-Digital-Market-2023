using NDE_Digital_Market.Model.DTO;

namespace NDE_Digital_Market.Services.GetBuyerInAdminService
{
    public interface IGetBuyerInAdmin_Service
    {
        Task<List<GetCompanySellerListDTO>> CompanySellerDetails(bool IsBuyer, bool IsActive);
    }
}
