using Microsoft.AspNetCore.Mvc;
using NDE_Digital_Market.Model;
using NDE_Digital_Market.SharedServices;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using NDE_Digital_Market.DTOs;
using NDE_Digital_Market.Services.ProductsService;
using NDE_Digital_Market.Model.DTO;

namespace NDE_Digital_Market.Controllers
{
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProducts_Service _products_Service;
        public ProductsController(IProducts_Service products_Service)
        {
            _products_Service = products_Service;
        }


        //===================================== Create User ================================


        [HttpPut]
        [Authorize(Roles = "seller")]
        [Route("UpdateProduct")]

        public async Task<IActionResult> UpdateProduct([FromForm] GoodsQuantityModel product)
        {
            try
            {
                if (product == null)
                {
                    return NotFound(new { message = "Give Valid Data." });
                }
                object res = await _products_Service.UpdateProduct(product);
                if (res == null)
                {
                    return NotFound(new { message = "No Data Found." });
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }
        }
        
        // ======================= GET Dashboard Contents ================== 

        [HttpGet ]
        [Route("GetDashboardContents")]

        public async Task<IActionResult> GetDashboardContents(string sellerCode, String? status = null, String? productName = null, String? companyName = null, DateTime? addedDate = null)
        {
            try
            {
                if (sellerCode == null )
                {
                    return NotFound(new { message = "Give Valid Data." });
                }
                object res = await _products_Service.GetDashboardContents( sellerCode, status, productName, companyName, addedDate);
                if (res == null)
                {
                    return NotFound(new { message = "No Data Found." });
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }
        }


        // ======================= GET Product ==================

        //[HttpGet]
        //[Route("GetProduct")]
        //public List<GoodsQuantityModel>GetSellerProduct(string sellerCode)
        //{
        //    Console.WriteLine(sellerCode, "sellerCode");
        //    string decryptedSupplierCode = CommonServices.DecryptPassword(sellerCode);

        //    string query = @"SELECT 
        //                    ProductList.GoodsId, 
        //                    ProductList.GoodsName, 
        //                    ProductList.GroupCode,
        //                    ProductList.GroupName,
        //                    ProductList.Specification,
        //                    ProductList.Price,
        //                    ProductList.SellerCode,
        //                    ProductList.ImagePath,
        //                    ISNULL(MaterialStockQty.PresentQty, 0) AS Quantity,
        //                    ProductList.QuantityUnit,  
        //                 UserRegistration.CompanyName

        //                 FROM ProductList
        //                LEFT JOIN
        //                UserRegistration
        //                ON
        //                ProductList.SellerCode = UserRegistration.UserCode
        //                LEFT JOIN
        //                                        MaterialStockQty
        //                                        ON
        //                                           MaterialStockQty.GroupCode = ProductList.GroupCode AND MaterialStockQty.GoodsId = ProductList.GoodsId
        //                WHERE ProductList.SellerCode = @DecryptedSupplierCode  ORDER BY ProductList.UpdatedDate DESC; ";
        //    SqlCommand cmd = new SqlCommand(query, con);
        //    cmd.Parameters.AddWithValue("@DecryptedSupplierCode", decryptedSupplierCode);

        //    con.Open();
        //    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        //    DataTable dt = new DataTable();

        //    adapter.Fill(dt);

        //    con.Close();

        //    List<GoodsQuantityModel> sellerProducts = new List<GoodsQuantityModel>();

        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        GoodsQuantityModel modelObj = new GoodsQuantityModel();


        //        modelObj.CompanyName = dt.Rows[i]["CompanyName"].ToString();
        //        modelObj.GroupCode = dt.Rows[i]["GroupCode"].ToString();
        //        modelObj.GoodsId = dt.Rows[i]["GoodsID"].ToString();
        //        modelObj.GroupName = dt.Rows[i]["GroupName"].ToString();
        //        modelObj.GoodsName = dt.Rows[i]["GoodsName"].ToString();
        //        modelObj.Specification = dt.Rows[i]["Specification"].ToString();
        //        modelObj.ApproveSalesQty = float.Parse(dt.Rows[i]["Quantity"].ToString());
        //        modelObj.SellerCode = dt.Rows[i]["SellerCode"].ToString();
        //        modelObj.Price = float.Parse(dt.Rows[i]["Price"].ToString());
        //        modelObj.QuantityUnit = dt.Rows[i]["QuantityUnit"].ToString();
        //        modelObj.ImagePath = dt.Rows[i]["ImagePath"].ToString();

        //        sellerProducts.Add(modelObj);

        //    }
        //    return sellerProducts;

        //}



        // ====================== new GET Product ==========================


        [Authorize(Roles ="admin")]
        [HttpGet("GetSellerProductForAdminApproval")]
        public async Task<IActionResult> GetSellerProductForAdminApproval(string status)
        {
            try
            {

                object res = await _products_Service.GetSellerProductForAdminApproval(status);
                if (res == null)
                {
                    return NotFound(new { message = "No Data Found." });
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }
        }

            // ======================= DELETE Product ==================

        [HttpDelete]
        [Authorize(Roles = "seller")]
        [Route("DeleteProduct")]
        public async Task<IActionResult> DeleteProcuct(string sellerCode, string ProductId)
        {
            try
            {
                if (sellerCode == null || ProductId == null)
                {
                    return NotFound(new { message = "Give Valid Data." });
                }
                object res = await _products_Service.DeleteProcuct(sellerCode, ProductId);
                if (res == null)
                {
                    return NotFound(new { message = "No Data Found." });
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }
        }


        //================== SellerProductPriceAndOffer status Update by Tushar ==================
        [Authorize(Roles = "admin")]
        [HttpPut("SellerProductStatusUpdate")]
        public async Task<IActionResult> UpdateSellerProductStatusAsync(List<UpdateSellerProductStatusDTO> productStatusList)
        {
            try
            {
                if (productStatusList == null)
                {
                    return NotFound(new { message = "Give Valid Data." });
                }
                object res = await _products_Service.UpdateSellerProductStatusAsync(productStatusList);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }
        }



        //[HttpPut]
        //[Route("UpdateProductStatus")]
        //public IActionResult UpdateProductStatus([FromForm] UpdateProductStatusModel Obj)
        //{

        //    string decryptedSupplierCode = CommonServices.DecryptPassword(Obj.userCode);
        //    bool cancelEdited = false;
        //    List<EditedUserInfoModel> users = new List<EditedUserInfoModel>();
        //    string UpdatedBy = decryptedSupplierCode;
        //    DateTime UpdateDate = DateTime.Now;
        //    //Console.WriteLine(Obj.productIDs);
        //    int statusBit = 1;
        //    if (Obj.status == "approved")
        //    {
        //        statusBit = 2;
        //    }
        //    if (Obj.status == "rejected") 
        //    {
        //        statusBit = 3; 
        //    }

        //    if (Obj.statusBefore == "edited")
        //    {
        //        StringBuilder queryEdited = new StringBuilder();
        //        if (Obj.status == "rejected")
        //        {
        //            cancelEdited = true;
        //            queryEdited = queryEdited.Append($"UPDATE ProductList  SET Status = 'approved', UpdatedBy = '{UpdatedBy}', UpdatedDate = '{UpdateDate}',UpdatedPc= '{Obj.updatedPC}'  WHERE GoodsId IN ({Obj.productIDs})");
        //            //queryEdited = queryEdited.Append($"SELECT SupplierCode,email,full_name FROM ProductList LEFT JOIN UserRegistration ON SupplierCode=UserCode WHERE ProductID IN ({Obj.productIDs})");
        //        }
        //        else
        //        {
        //            // Split the product IDs into an array

        //            string[] productIDsArray = Obj.productIDs.Split(',');

        //            // Loop through each product ID
        //            foreach (string productId in productIDsArray)
        //            {

        //                queryEdited.Append($"UPDATE ProductList SET GoodsName = EPL.GoodsName, Specification = EPL.Specification, " +
        //                $"GroupCode = EPL.GroupCode,GroupName = EPL.GroupName," +
        //                $"Price = EPL.Price,Quantity = EPL.Quantity,QuantityUnit" +
        //                $" = EPL.QuantityUnit, ");

        //                queryEdited.Append($"Status = '{Obj.status}',  UpdatedBy = '{UpdatedBy}', UpdatedDate = '{UpdateDate}',updatedPc= '{Obj.updatedPC}'");
        //                queryEdited.Append($"FROM (SELECT * FROM EditedProductList WHERE GoodsId = {productId}) AS EPL ");
        //                queryEdited.Append($"WHERE ProductList.Goodsid = {productId} AND ProductList.SellerCode = EPL.SellerCode;");


        //            }
        //        }

        //        StringBuilder deleteQuery = new StringBuilder();
        //        deleteQuery.Append($"DELETE FROM EditedProductList WHERE GoodsId IN ({Obj.productIDs})");
        //        string combinedQuery = queryEdited.ToString() + deleteQuery.ToString();
        //        SqlCommand cmdEdite = new SqlCommand(combinedQuery, con);
        //        con.Open();
        //        cmdEdite.CommandType = CommandType.Text;
        //        cmdEdite.ExecuteNonQuery();

        //        string selectQuery = $"SELECT SellerCode, Email, FullName,GoodsName FROM ProductList LEFT JOIN UserRegistration ON SellerCode=UserCode WHERE GoodsId IN ({Obj.productIDs})";
        //        using (SqlCommand selectCmd = new SqlCommand(selectQuery, con))
        //        {
        //            selectCmd.CommandType = CommandType.Text;

        //            using (SqlDataReader reader = selectCmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    EditedUserInfoModel user = new EditedUserInfoModel();
        //                    user.SupplierCode = reader["SellerCode"].ToString();
        //                    user.Email = reader["Email"].ToString();
        //                    user.FullName = reader["FullName"].ToString();
        //                    user.ProductName = reader["GoodsName"].ToString();
        //                    users.Add(user);
        //                }
        //                // Now you can use the 'users' list with the retrieved data.
        //            }

        //        }
        //        con.Close();



        //    }
        //    else
        //    {
        //        string query = $"UPDATE ProductList  SET Status = '{Obj.status}', UpdatedBy = '{UpdatedBy}', UpdatedDate = '{UpdateDate}',UpdatedPc= '{Obj.updatedPC}'  WHERE GoodsId IN ({Obj.productIDs})";
        //        SqlCommand cmd = new SqlCommand(query, con);

        //        con.Open();
        //        cmd.CommandType = CommandType.Text;
        //        cmd.ExecuteNonQuery();
        //        con.Close();
        //    }

        //    return Ok(new { message = "Product status updated successfully", cancelEdited, users });
        //}




        [HttpGet ]
        [Route("comapreEditedProduct")]
        public async Task<IActionResult> comapreEditedProduct(string productId)
        {
            try
            {
                if (productId == null)
                {
                    return NotFound(new { message = "Give Valid Data." });
                }
                object res = await _products_Service.comapreEditedProduct(productId);
                if (res == null)
                {
                    return NotFound(new { message = "No Data Found." });
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Server Error. Try Again!!!" });
            }
        }

    }
}
