
using NDE_Digital_Market.Model;
using NDE_Digital_Market.SharedServices;
using System.Data;
using System.Data.SqlClient;

namespace NDE_Digital_Market.Data_Access_Layer;

public class ProductReturn_DAL
{
    private readonly string _healthCareConnection;
    private readonly IConfiguration _configuration;

    public ProductReturn_DAL(IConfiguration config)
    {
        _configuration = config;
        CommonServices commonServices = new CommonServices(config);
        _healthCareConnection = commonServices.HealthCareConnection;
    }

    public async Task<object> InsertProductReturn(ProductReturnModel returnData)
    {
        SqlTransaction transaction = null;
        SqlConnection con = new SqlConnection(_healthCareConnection);

        try
        {
            await con.OpenAsync();
            transaction = (SqlTransaction)await con.BeginTransactionAsync();
            string systemCode = string.Empty;

            SqlCommand cmdSP = new SqlCommand("spMakeSystemCode", con, transaction);
            {
                cmdSP.CommandType = CommandType.StoredProcedure;
                cmdSP.Parameters.AddWithValue("@TableName", "ProductReturn");
                cmdSP.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
                cmdSP.Parameters.AddWithValue("@AddNumber", 1);
                var tempSystem = await cmdSP.ExecuteScalarAsync();

                systemCode = tempSystem?.ToString() ?? string.Empty;
            }
            int ProductReturnId = int.Parse(systemCode.Split('%')[0]);
            string ProductReturnCode = systemCode.Split('%')[1];

            string query = @"INSERT INTO ProductReturn(ProductReturnId,ProductReturnCode,ReturnTypeId,ProductGroupId,ProductId,OrderNo,Price,
                                    OrderDetailsId,SellerId,ApplyDate,DeliveryDate,Remarks,AddedDate,AddedBy,AddedPc)
                                VALUES(@ProductReturnId,@ProductReturnCode,@ReturnTypeId,@ProductGroupId,@ProductId,@OrderNo,@Price,
                                    @OrderDetailsId,@SellerId,@ApplyDate,@DeliveryDate,@Remarks,@AddedDate,@AddedBy,@AddedPc);";


            SqlCommand cmd = new SqlCommand(query, con, transaction);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.AddWithValue("@ProductReturnId", ProductReturnId);
            cmd.Parameters.AddWithValue("@ProductReturnCode", ProductReturnCode);
            cmd.Parameters.AddWithValue("@ReturnTypeId", returnData.ReturnTypeId);
            cmd.Parameters.AddWithValue("@ProductGroupId", returnData.ProductGroupId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ProductId", returnData.ProductId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@OrderNo", returnData.OrderNo ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Price", returnData.Price ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@OrderDetailsId", returnData.OrderDetailsId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@SellerId", returnData.SellerId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ApplyDate", DateTime.Now);
            cmd.Parameters.AddWithValue("@DeliveryDate", returnData.DeliveryDate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Remarks", returnData.Remarks ?? (object)DBNull.Value);

            cmd.Parameters.AddWithValue("@AddedDate", DateTime.Now);
            cmd.Parameters.AddWithValue("@AddedBy", returnData.AddedBy ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@AddedPc", returnData.AddedPc ?? (object)DBNull.Value);

            int a = await cmd.ExecuteNonQueryAsync();
            if (a > 0)
            {
                SqlCommand command = new SqlCommand("UPDATE OrderDetails SET Status = 'ToReturn' WHERE OrderDetailId = " + returnData.OrderDetailsId + "", con, transaction);

                int updateResult = await command.ExecuteNonQueryAsync();

                if (updateResult <= 0)
                {

                    await transaction.RollbackAsync();
                    return (new { message = "Order Details status isn't change or not found." });
                }
            }
            else
            {
                return (new { message = "ProductReturn data isn't Inserted Successfully." });
            }

            await transaction.CommitAsync();
            return (new { message = "ProductReturn data Inserted Successfully." });

        }
        catch (Exception ex)
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync();
            }
            return (new { message = ex.Message });
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                await con.CloseAsync();
            }
        }

    }

}
