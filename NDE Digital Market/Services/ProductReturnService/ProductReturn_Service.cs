using NDE_Digital_Market.Data_Access_Layer;
using NDE_Digital_Market.Model;
using NDE_Digital_Market.Model.DTO;
using NDE_Digital_Market.SharedServices;

namespace NDE_Digital_Market.Services.ProductReturnService;

public class ProductReturn_Service : IProductReturn_Service
{
    private readonly ProductReturn_DAL _productReturn_DAL;
    public ProductReturn_Service(ProductReturn_DAL productReturn_DAL)
    {
        _productReturn_DAL = productReturn_DAL;
    }

    public async Task<object> InsertProductReturn(InsertProductReturnDTO returnData)
    {

        ProductReturnModel Model = new ProductReturnModel();
        Model.ProductId = int.Parse(CommonServices.DecryptPassword(returnData.ProductId));
        Model.ProductGroupId = int.Parse(CommonServices.DecryptPassword(returnData.ProductGroupId));
        Model.ReturnTypeId = int.Parse(CommonServices.DecryptPassword(returnData.ReturnTypeId));
        Model.OrderNo = CommonServices.DecryptPassword(returnData.OrderNo);
        Model.Price = returnData.Price;
        Model.OrderDetailsId = int.Parse(CommonServices.DecryptPassword(returnData.OrderDetailsId));
        Model.SellerId = int.Parse(CommonServices.DecryptPassword(returnData.SellerId));
        Model.DeliveryDate = returnData.DeliveryDate;
        Model.Remarks = returnData.Remarks;
        Model.ApplyDate = DateTime.UtcNow;
        Model.AddedDate = DateTime.UtcNow;

        return await _productReturn_DAL.InsertProductReturn(Model);
    }

}
