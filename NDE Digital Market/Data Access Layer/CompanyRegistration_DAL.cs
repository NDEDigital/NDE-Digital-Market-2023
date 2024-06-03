using NDE_Digital_Market.Data_Access_Layer;
using NDE_Digital_Market.Model;
using NDE_Digital_Market.DTOs;
using System.Data;
using System.Data.SqlClient;
using NDE_Digital_Market.SharedServices;
using NDE_Digital_Market.Controllers;

namespace NDE_Digital_Market.Data_Access_Layer;

public class CompanyRegistration_DAL
{
    //CommonServices commonServices = new CommonServices(_configuration);
    //string foldername = commonServices.FilesPath + "CompanyFiles";

    private readonly IConfiguration _configuration;
    private readonly SqlConnection connection;
    private readonly string foldername;
    private readonly string filename = "companyfiles";

    private readonly string _healthCareConnection;

    public CompanyRegistration_DAL(IConfiguration configuration)
    {
        _configuration = configuration;
        CommonServices commonServices = new CommonServices(_configuration);
        connection = new SqlConnection(commonServices.HealthCareConnection);
       
        foldername = commonServices.FilesPath + "CompanyFiles";

        _healthCareConnection = commonServices.HealthCareConnection;

    }
    public async Task<Boolean> CompanyExistAsync(CompanyDto companyDto)
    {
        SqlCommand cmd = new SqlCommand("CheckCompanyExistence", connection);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@CompanyName", companyDto.CompanyName);
        cmd.Parameters.AddWithValue("@BusinessRegistrationNumber", companyDto.BusinessRegistrationNumber);
        cmd.Parameters.AddWithValue("@TaxIdentificationNumber", companyDto.TaxIdentificationNumber);
        await connection.OpenAsync();
        int count = (int)await cmd.ExecuteScalarAsync();
        await connection.CloseAsync();
        Boolean companyNameExist = false;
        if (count > 0)
        {
            companyNameExist = true;
        }
        return companyNameExist;
        //   return BadRequest(new { message = "User does not exist" , userExist });
    }

    public async Task<string> CompanyRegistrationPostAsync(CompanyDto companyDto)
    {
        Boolean companyNameExist = await CompanyExistAsync(companyDto);

        if (companyNameExist)
        {
            return null;
        }
        else
        {
            string systemCode = string.Empty;

            // Execute the stored procedure to generate the system code
            SqlCommand cmdSP = new SqlCommand("spMakeSystemCode", connection);
            {
                cmdSP.CommandType = CommandType.StoredProcedure;
                cmdSP.Parameters.AddWithValue("@TableName", "CompanyRegistration");
                cmdSP.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
                cmdSP.Parameters.AddWithValue("@AddNumber", 1);

                //con.Open();
                //using (SqlDataReader reader = cmdSP.ExecuteReader())
                //{
                //    if (reader.Read())
                //    {
                //        systemCode = reader["SystemCode"].ToString();
                //    }
                //}
                //con.Close();
                await connection.OpenAsync();
                var tempSystem = await cmdSP.ExecuteScalarAsync();
                systemCode = tempSystem?.ToString() ?? string.Empty;
                await connection.CloseAsync();
            }

            string CompanyImage = CommonServices.UploadFiles(foldername, filename, companyDto.CompanyImageFile);
            string TradeLicense = CommonServices.UploadFiles(foldername, filename, companyDto.TradeLicenseFile);
            int CompanyID = int.Parse(systemCode.Split('%')[0]);
            string CompanyCode = systemCode.Split('%')[1];
            //SP END

            SqlCommand cmd = new SqlCommand("INSERT INTO CompanyRegistration (CompanyID, CompanyCode, CompanyName,Email, CompanyImage, " +
                           "CompanyFoundationDate, BusinessRegistrationNumber, TaxIdentificationNumber, " +
                           "TradeLicense, PreferredPaymentMethodID, BankNameID, AccountNumber, " +
                           "AccountHolderName,MaxUser,IsActive, AddedBy, DateAdded, AddedPC) " +
                           "VALUES (@CompanyID, @CompanyCode, @CompanyName,@Email, @CompanyImage, " +
                           "@CompanyFoundationDate, @BusinessRegistrationNumber, @TaxIdentificationNumber, " +
                           "@TradeLicense, @PreferredPaymentMethodID, @BankNameID, @AccountNumber, " +
                           "@AccountHolderName, @MaxUser, @IsActive, @AddedBy, @DateAdded, @AddedPC);", connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@CompanyID", CompanyID);
            cmd.Parameters.AddWithValue("@CompanyCode", CompanyCode);
            cmd.Parameters.AddWithValue("@CompanyName", companyDto.CompanyName);
            cmd.Parameters.AddWithValue("@Email", companyDto.Email);
            cmd.Parameters.AddWithValue("@CompanyImage", CompanyImage);
            cmd.Parameters.AddWithValue("@CompanyFoundationDate", companyDto.CompanyFoundationDate);
            cmd.Parameters.AddWithValue("@BusinessRegistrationNumber", companyDto.BusinessRegistrationNumber);
            cmd.Parameters.AddWithValue("@TaxIdentificationNumber", companyDto.TaxIdentificationNumber);
            cmd.Parameters.AddWithValue("@TradeLicense", TradeLicense);
            cmd.Parameters.AddWithValue("@PreferredPaymentMethodID", companyDto.PreferredPaymentMethodID);
            cmd.Parameters.AddWithValue("@BankNameID", companyDto.BankNameID ?? 0);
            cmd.Parameters.AddWithValue("@AccountNumber", companyDto.AccountNumber ?? string.Empty);
            cmd.Parameters.AddWithValue("@AccountHolderName", companyDto.AccountHolderName ?? string.Empty);
            cmd.Parameters.AddWithValue("@MaxUser", 3);
            cmd.Parameters.AddWithValue("@IsActive", -1);
            cmd.Parameters.AddWithValue("@AddedBy", companyDto.AddedBy);
            cmd.Parameters.AddWithValue("@DateAdded", DateTime.Now);
            cmd.Parameters.AddWithValue("@AddedPC", companyDto.AddedPC);

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            await connection.CloseAsync();

            return "Company Registration successfull!.";
        }



    }

    public async Task<DataTable> GetCompaniesAsync(int status)
    {

        try
        {

            DataTable dataTable = new DataTable();
            using (SqlConnection con = new SqlConnection(_healthCareConnection))
            {
                string query = @"GetCompaniesByStatus";

                await con.OpenAsync();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IsActive", status);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }

                }
            }
            return dataTable;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            // You might want to handle errors more gracefully
            return null;
        }
    }

    public async Task<string> UpdateCompanyAsync(CompanyDto companyDto)
    {


        string updateSql = "UPDATE CompanyRegistration SET ";
        if (companyDto.IsActive != null)
        {
            updateSql += " IsActive = @IsActive,";
        }
        if (companyDto.MaxUser != null)
        {
            updateSql += " MaxUser = @MaxUser,";
        }
        updateSql += " DateUpdated = @DateUpdated,";
        updateSql = updateSql.TrimEnd(',');
  
        updateSql += " WHERE CompanyCode = @CompanyCode;";
        SqlCommand cmd = new SqlCommand(updateSql, connection);
        if (companyDto.IsActive != null)
        {
            cmd.Parameters.AddWithValue("@IsActive", companyDto.IsActive);
        }
        if (companyDto.MaxUser != null)
        {
            cmd.Parameters.AddWithValue("@MaxUser", companyDto.MaxUser);
        }
        cmd.Parameters.AddWithValue("@CompanyCode", companyDto.CompanyCode);
        cmd.Parameters.AddWithValue("@DateUpdated", DateTime.Now);
        await connection.OpenAsync();
        // Execute the update
        int res = await cmd.ExecuteNonQueryAsync();
        await connection.CloseAsync();

        if (res > 0)
        {
            if (companyDto.IsActive != null)
            {
                if (companyDto.IsActive == 1)
                {
                    return "Company is Active Now.";
                }
                else if (companyDto.IsActive == 0)
                {
                    return "Company is InActive Now.";
                }

            }

        }
        return "there is a error";

    }
}
