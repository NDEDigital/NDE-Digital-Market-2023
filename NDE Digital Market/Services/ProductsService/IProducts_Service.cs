using NDE_Digital_Market.Model;
using NDE_Digital_Market.Model.DTO;

namespace NDE_Digital_Market.Services.ProductsService
{
    public interface IProducts_Service
    {
        Task<object> UpdateProduct(GoodsQuantityModel product);

        Task<object> GetDashboardContents(string sellerCode, String? status = null, String? productName = null, String? companyName = null, DateTime? addedDate = null);


        Task<List<GetSellerProductListForAdminApprovalDTO>> GetSellerProductForAdminApproval(string status);


        Task<object> DeleteProcuct(string sellerCode, string ProductId);


        Task<object> UpdateSellerProductStatusAsync(List<UpdateSellerProductStatusDTO> productStatusList);



        Task<object> comapreEditedProduct(string productId);


    }
}
