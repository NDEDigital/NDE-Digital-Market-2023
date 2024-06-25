using NDE_Digital_Market.SharedServices;
using System.Data;
using System.Data.SqlClient;

namespace NDE_Digital_Market.Data_Access_Layer
{
    public class SellerInventory_DAL
    {
        private readonly string _healthCareConnection;

        public SellerInventory_DAL(IConfiguration config)
        {
            CommonServices commonServices = new CommonServices(config);
            _healthCareConnection = commonServices.HealthCareConnection;
        }

        public async Task<DataTable> GetSellerInventoryDataBySellerId(int UserId)
        {
            SqlConnection con = new SqlConnection(_healthCareConnection);
            DataTable dataTable = new DataTable();

            try
            {
                    await con.OpenAsync();
                    string query = "GetSellerInvantoryDataBySellerId";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserId", UserId);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                return dataTable;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred on GetWishList: {ex.Message}");
                return null;
            }
            finally
            {
                await con.CloseAsync();
            }
        }

    }

}
