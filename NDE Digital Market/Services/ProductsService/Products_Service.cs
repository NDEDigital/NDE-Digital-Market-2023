using NDE_Digital_Market.Data_Access_Layer;
using NDE_Digital_Market.DTOs;
using NDE_Digital_Market.Model;
using NDE_Digital_Market.Model.DTO;
using NDE_Digital_Market.SharedServices;

namespace NDE_Digital_Market.Services.ProductsService;

public class Products_Service : IProducts_Service
{
    private readonly Products_DAL _product_DAL;
    public Products_Service(Products_DAL products_DAL)
    {
        _product_DAL = products_DAL;
    }

    public async Task<object> UpdateProduct(GoodsQuantityModel product)
    {
        return await _product_DAL.UpdateProduct(product);
    }


    public async Task<object> GetDashboardContents(string sellerCode, String? status = null, String? productName = null, String? companyName = null, DateTime? addedDate = null)
    {
        return await _product_DAL.GetDashboardContents(sellerCode, status, productName, companyName, addedDate);
    }


    public async Task<List<GetSellerProductListForAdminApprovalDTO>> GetSellerProductForAdminApproval(string status)
    {
        return await _product_DAL.GetSellerProductForAdminApproval(status);
    }


    public async Task<object> DeleteProcuct(string sellerCode, string ProductId)
    {
        int decryptedProductId = int.Parse(CommonServices.DecryptPassword(ProductId));
        return await _product_DAL.DeleteProcuct(sellerCode, decryptedProductId);
    }


    public async Task<object> UpdateSellerProductStatusAsync(List<UpdateSellerProductStatusDTO> productStatusList)
    {
        List<SellerProductsModel> modellist = new List<SellerProductsModel>();
        for (int i=0; i < productStatusList.Count; i++)
        {
            SellerProductsModel model = new SellerProductsModel();

            model.ProductId = int.Parse(CommonServices.DecryptPassword(productStatusList[i].ProductId));
            model.CompanyCode = CommonServices.DecryptPassword(productStatusList[i].CompanyCode);
            model.Status = productStatusList[i].Status;
            model.UserId = int.Parse(CommonServices.DecryptPassword(productStatusList[i].UserId));

            modellist.Add(model);
        }
        return await _product_DAL.UpdateSellerProductStatusAsync(modellist);
    }



    public async Task<object> comapreEditedProduct(string productId)
    {
        int DecryptProductId = int.Parse(CommonServices.DecryptPassword(productId));
        return await _product_DAL.comapreEditedProduct(DecryptProductId);
    }

}
