using NDE_Digital_Market.Data_Access_Layer;
using NDE_Digital_Market.SharedServices;
using System.Data;
using NDE_Digital_Market.Model.DTO;
using NDE_Digital_Market.Model;

namespace NDE_Digital_Market.Services.CompanyRegistrationServices;

public class CompanyRegistration : ICompanyRegistration
{
    private readonly CompanyRegistration_DAL _CompanyRegistration_DAL;
    private readonly string foldername = "CompanyFiles";
    public CompanyRegistration(CompanyRegistration_DAL companyRegistration_DAL)
    {
        this._CompanyRegistration_DAL = companyRegistration_DAL;
    }


    public async Task<Boolean> CompanyexistsCheckAsync(CompanyModel companyDto)
    {
        Boolean res = await _CompanyRegistration_DAL.CompanyExistAsync(companyDto);
        return res;
    }


    public async Task<string> CompanyRegistrationPostAsync(CompanyModel companyDto)
    {
        string res = await _CompanyRegistration_DAL.CompanyRegistrationPostAsync(companyDto);
        return res;
    }

    public async Task<List<GetCompanyListByStatusDTO>> GetCompaniesAsync(int status)
    {

        DataTable dataTable = await _CompanyRegistration_DAL.GetCompaniesAsync(status);

        List<GetCompanyListByStatusDTO> list = new List<GetCompanyListByStatusDTO>();
        // Check if dataTable is null
        if (dataTable == null)
        {
            return null;
        }

        foreach (DataRow row in dataTable.Rows)
        {
            GetCompanyListByStatusDTO company = new GetCompanyListByStatusDTO();

            company.CompanyID = CommonServices.EncryptPassword(row["CompanyID"].ToString());
            company.MaxUser = Convert.ToInt32(row["MaxUser"]);
            company.CompanyCode = row["CompanyCode"].ToString();
            company.CompanyName = row["CompanyName"].ToString();
            company.Email = row["Email"].ToString();
            company.CompanyAdminId = CommonServices.EncryptPassword(row["CompanyAdminId"].ToString());
            company.CompanyImage = row["CompanyImage"].ToString();
            company.CompanyFoundationDate = Convert.ToDateTime(row["CompanyFoundationDate"]);
            company.BusinessRegistrationNumber = row["BusinessRegistrationNumber"].ToString();
            company.TaxIdentificationNumber = row["TaxIdentificationNumber"].ToString();
            company.TradeLicense = row["TradeLicense"].ToString();
            company.PreferredPaymentMethodID = CommonServices.EncryptPassword(row["PreferredPaymentMethodID"].ToString());
            company.PreferredPaymentMethodName = row["PreferredPaymentMethodName"].ToString();
            company.BankNameID = CommonServices.EncryptPassword(row["BankNameID"].ToString());
            company.BankName = row["BankName"].ToString();
            company.AccountNumber = row["AccountNumber"].ToString();
            company.AccountHolderName = row["AccountHolderName"].ToString();

            list.Add(company);
        }

        return list;

    }
    public async Task<string> UpdateCompanyAsync(CompanyModel companyDto)
    {
        var res = await _CompanyRegistration_DAL.UpdateCompanyAsync(companyDto);
        return res;
    }
}
