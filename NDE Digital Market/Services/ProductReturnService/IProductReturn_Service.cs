using NDE_Digital_Market.Model.DTO;

namespace NDE_Digital_Market.Services.ProductReturnService;

public interface IProductReturn_Service
{
    Task<object> InsertProductReturn(InsertProductReturnDTO returnData);
}
