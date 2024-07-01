using NDE_Digital_Market.SharedServices;
using System.Data;
using System.Data.SqlClient;

namespace NDE_Digital_Market.Data_Access_Layer
{
    public class Dashboard_DAL
    {
        private readonly string _healthCareConnection;
        public Dashboard_DAL(IConfiguration configuration)
        {
            CommonServices commonServices = new CommonServices(configuration);
            _healthCareConnection = commonServices.HealthCareConnection;
        }


        public async Task<DataTable> CompanySellerDetails(int UserId)
        {

            try
            { 

                DataTable dataTable = new DataTable();
                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    string query = @"SELECT M.MenuId,M.MenuName,M.IsActive
                                                FROM MenuList M
                                                LEFT JOIN Permission P ON M.MenuId = P.MenuId AND P.UserId = @UserId
                                                WHERE M.IsActive=1 AND P.MenuId IS NULL AND M.IsAdmin!=1;";

                    await con.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@UserId", UserId);
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

    }
}
