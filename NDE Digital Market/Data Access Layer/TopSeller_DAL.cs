using NDE_Digital_Market.SharedServices;
using System.Data;
using System.Data.SqlClient;

namespace NDE_Digital_Market.Data_Access_Layer
{
    public class TopSeller_DAL
    {
        private readonly string _healthCareConnection;

        public TopSeller_DAL(IConfiguration config)
        {
            CommonServices commonServices = new CommonServices(config);
            _healthCareConnection = commonServices.HealthCareConnection;
        }



        public async Task<DataTable> getForDropDown()
        {
            SqlConnection con = new SqlConnection(_healthCareConnection);
            DataTable dataTable = new DataTable();

            try
            {
                await con.OpenAsync();
                string query = "GetTopSellerCompanies";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    await con.CloseAsync();
                }
            }
            return dataTable;
        }
    }
}
