using NDE_Digital_Market.Data_Access_Layer;
using NDE_Digital_Market.Model;
using System.Data.SqlClient;
using NDE_Digital_Market.SharedServices;
using System.Data;
namespace NDE_Digital_Market.Data_Access_Layer;

public class HK_Gets_DAL
{

    private readonly string _healthCareConnection;
    public HK_Gets_DAL(IConfiguration configuration)
    {
        CommonServices commonServices = new CommonServices(configuration);
        _healthCareConnection = commonServices.HealthCareConnection;
    }
    public async Task<DataTable> PaymentMethodGetAsync()
    {

        try
        {
            DataTable dataTable = new DataTable();
            string query = @"select PMMasterID as PMID, PMName from HK_PaymentMethodMaster;";

            using (SqlConnection con = new SqlConnection(_healthCareConnection))
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
                await con.CloseAsync();
            }
            return dataTable;
        }
        catch (Exception ex)
        {
            return null;
        }
    }


    public async Task<DataTable> BankNameGetAsync(int preferredPM)
    {
        try
        {
            DataTable dataTable = new DataTable();
            string query = @"select PMDetailsID as PMID, PMBankName as PMName from HK_PaymentMethodDetails where PMMasterID = @preferredPM;";

            using (SqlConnection con = new SqlConnection(_healthCareConnection))
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@preferredPM", preferredPM);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
                await con.CloseAsync();
            }
            return dataTable;
        }
        catch (Exception ex)
        {
            return null;
        }
    }



    public async Task<DataTable> GetReturnListAsync()
    {

        try
        {
            DataTable dataTable = new DataTable();
            string query = @"select ReturnTypeId, ReturnTypeName from HK_ReturnType;";

            using (SqlConnection con = new SqlConnection(_healthCareConnection))
            {
                await con.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
                await con.CloseAsync();
            }
            return dataTable;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

}
