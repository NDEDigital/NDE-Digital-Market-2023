using NDE_Digital_Market.Model;
using NDE_Digital_Market.SharedServices;
using System.Data;
using System.Data.SqlClient;

namespace NDE_Digital_Market.Data_Access_Layer
{
    public class AddToCart_DAL
    {


        private readonly string _healthCareConnection;
        public AddToCart_DAL(IConfiguration configuration)
        {
            CommonServices commonServices = new CommonServices(configuration);
            _healthCareConnection = commonServices.HealthCareConnection;
        }


        public async Task<DataTable> GetAddToCartData()
        {

            try
            {
                DataTable dataTable = new DataTable();
                string query = @"SELECT* FROM AddToCart";

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



        public async Task<DataTable> GetAddToCartDataByUserID(int userId)
        {

            try
            {
                DataTable dataTable = new DataTable();
                string query = @"GetAddToCartDataByBuyerUserId";

                using (SqlConnection con = new SqlConnection(_healthCareConnection))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@BuyerUserId", userId);
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



        public async Task<object> AddToCartData(AddToCartModal addToCart)
        {
            try
            {
                using (var con = new SqlConnection(_healthCareConnection))
                {
                    con.Open();

                    var checkCmd = new SqlCommand("SELECT COUNT(*) FROM AddToCart WHERE BuyerUserID = @BuyerUserID AND ProductID = @ProductID AND CompanyCode = @CompanyCode ", con);
                    checkCmd.Parameters.AddWithValue("@BuyerUserID", addToCart.BuyerUserID);
                    checkCmd.Parameters.AddWithValue("@ProductID", addToCart.ProductID);
                    checkCmd.Parameters.AddWithValue("@CompanyCode", addToCart.CompanyCode);


                    int exists = (int)checkCmd.ExecuteScalar();

                    if (exists > 0)
                    {
                        // If the record exists, update only the ProductCartQuantity
                        var updateCmd = new SqlCommand("UPDATE AddToCart SET ProductCartQuantity = @ProductCartQuantity WHERE BuyerUserID = @BuyerUserID AND ProductID = @ProductID AND CompanyCode = @CompanyCode", con);
                        updateCmd.Parameters.AddWithValue("@BuyerUserID", addToCart.BuyerUserID);
                        updateCmd.Parameters.AddWithValue("@ProductID", addToCart.ProductID);
                        updateCmd.Parameters.AddWithValue("@CompanyCode", addToCart.CompanyCode);
                        updateCmd.Parameters.AddWithValue("@ProductCartQuantity", addToCart.ProductCartQuantity);

                        updateCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        // If the record does not exist, insert the new record
                        var insertCmd = new SqlCommand(@"INSERT INTO AddToCart (BuyerUserID, CompanyCode, ProductID, ProductGroupID, UnitID, ProductCartQuantity, AddedDate, AddedBy, AddedPC)
                                    VALUES (@BuyerUserID, @CompanyCode, @ProductID, @ProductGroupID, @UnitID, @ProductCartQuantity, @AddedDate, @AddedBy, @AddedPC)", con);

                        insertCmd.Parameters.AddWithValue("@BuyerUserID", addToCart.BuyerUserID);
                        insertCmd.Parameters.AddWithValue("@CompanyCode", addToCart.CompanyCode);
                        insertCmd.Parameters.AddWithValue("@ProductID", addToCart.ProductID);
                        insertCmd.Parameters.AddWithValue("@ProductGroupID", addToCart.ProductGroupID);
                        insertCmd.Parameters.AddWithValue("@UnitID", addToCart.UnitID);
                        insertCmd.Parameters.AddWithValue("@ProductCartQuantity", addToCart.ProductCartQuantity);
                        insertCmd.Parameters.AddWithValue("@AddedDate", DateTime.UtcNow);
                        insertCmd.Parameters.AddWithValue("@AddedBy", addToCart.AddedBy);
                        insertCmd.Parameters.AddWithValue("@AddedPC", addToCart.AddedPC);

                        insertCmd.ExecuteNonQuery();
                    }
                }

                return (new { message = "Data added or updated successfully.", result = true });
            }
            catch (Exception ex)
            {
                return (new { message = "Error occurred while adding or updating data.", error = ex.Message, result = false });
            }
        }



        public async Task<object> DeleteAddToCart(int id)
        {
            try
            {
                using (var con = new SqlConnection(_healthCareConnection))
                {
                    con.Open();

                    // Check if the record exists
                    var checkCmd = new SqlCommand("SELECT COUNT(*) FROM AddToCart WHERE Id = @Id", con);
                    checkCmd.Parameters.AddWithValue("@Id", id);

                    int exists = (int)checkCmd.ExecuteScalar();

                    if (exists > 0)
                    {
                        // If the record exists, delete it
                        var deleteCmd = new SqlCommand("DELETE FROM AddToCart WHERE Id = @Id", con);
                        deleteCmd.Parameters.AddWithValue("@Id", id);

                        deleteCmd.ExecuteNonQuery();

                        return (new { message = "Data deleted successfully.", result = true });
                    }
                    else
                    {
                        // If the record does not exist, return a NotFound response
                        return (new { message = "Record not found.", result = false });
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return (new { message = "Error occurred while deleting data.", error = ex.Message, result = false });
            }
        }


    }
}
