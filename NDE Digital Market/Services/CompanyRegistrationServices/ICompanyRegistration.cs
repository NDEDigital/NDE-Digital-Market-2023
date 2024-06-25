
using NDE_Digital_Market.Model;
using NDE_Digital_Market.Model.DTO;

namespace NDE_Digital_Market.Services.CompanyRegistrationServices;

public interface ICompanyRegistration
{
    Task<string> CompanyRegistrationPostAsync(CompanyModel companyDto);
    Task<Boolean> CompanyexistsCheckAsync(CompanyModel companyDto);

    Task<List<GetCompanyListByStatusDTO>> GetCompaniesAsync(int status);

    Task<string> UpdateCompanyAsync(CompanyModel companyDto);

}
