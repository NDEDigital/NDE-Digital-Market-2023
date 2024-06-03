using NDE_Digital_Market.SharedServices;
using System.Data;
using System.Data.SqlClient;

namespace NDE_Digital_Market.Data_Access_Layer
{
    public class Invoice_DAL
    {
        private readonly string _healthCareConnection;
        public Invoice_DAL(IConfiguration configuration)
        {
            CommonServices commonServices = new CommonServices(configuration);
            _healthCareConnection = commonServices.HealthCareConnection;
        }

        public async Task<DataSet> GetInvoiceDataForBuyer(int OrderMasterId)
        {

            try
            {
                DataSet dataSet = new DataSet(); // Use DataSet instead of DataTable
                string query = "GetOrderInvoiceByMasterId";
                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@OrderMasterId", OrderMasterId);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataSet); // Fill the DataSet
                        }
                    }
                    await con.CloseAsync();
                }
                return dataSet; // Return the DataSet containing multiple tables
            }
            catch (Exception ex)
            {
                return null;
            }

        }



        public async Task<DataTable> GetInvoiceDataForSeller(int SSMId)
        {

            try
            {
                DataTable dataTable = new DataTable();
                string query = "SellerInvoice";
                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SSMId", SSMId);

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
