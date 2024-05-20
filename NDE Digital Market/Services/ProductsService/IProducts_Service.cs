namespace NDE_Digital_Market.Services.ProductsService
{
    public interface IProducts_Service
    {

        //UpdateProduct([FromForm] GoodsQuantityModel product);

        // ======================= GET Dashboard Contents ================== 

        //GetDashboardContents(string sellerCode, String? status = null, String? productName = null, String? companyName = null, DateTime? addedDate = null);



        // ====================== new GET Product ==========================

        //Task<IActionResult> GetSellerProductForAdminApproval(string status);

        // ======================= DELETE Product ==================

        //DeleteProcuct(string sellerCode, int ProductId);

        //================== SellerProductPriceAndOffer status Update by Tushar ==================

        //Task<IActionResult> UpdateSellerProductStatusAsync(List<ProductStatusDto> productStatusList);




        //public class EditedUserInfoModel
        //{

        //    public string? FullName { get; set; }
        //    public string? SupplierCode { get; set; }
        //    public string? Email { get; set; }
        //    public string? ProductName { get; set; }
        //}


        //comapreEditedProduct(int productId);
    }
}
