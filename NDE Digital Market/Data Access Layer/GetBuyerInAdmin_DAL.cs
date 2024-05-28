
using NDE_Digital_Market.SharedServices;
using System.Data;
using System.Data.SqlClient;

namespace NDE_Digital_Market.Data_Access_Layer
{
    public class GetBuyerInAdmin_DAL
    {
        private readonly string _healthCareConnection;
        public GetBuyerInAdmin_DAL(IConfiguration configuration)
        {
            CommonServices commonServices = new CommonServices(configuration);
            _healthCareConnection = commonServices.HealthCareConnection;
        }
        public async Task<DataTable> CompanySellerDetails(bool IsBuyer, bool IsActive)
        {
            try
            {
                DataTable dataTable = new DataTable();
                string query = @"SELECT UR.UserId, UR.FullName, UR.PhoneNumber, UR.Email, UR.Address, UR.AddedDate, UR.IsActive, UR.CompanyCode, UR.IsBuyer
                                            FROM UserRegistration UR
                                            WHERE (UR.IsBuyer = 1 AND UR.IsActive = @IsActive);";
                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@IsActive", IsActive);
                        cmd.Parameters.AddWithValue("@IsBuyer", IsBuyer);

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
}
