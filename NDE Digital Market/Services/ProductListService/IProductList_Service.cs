using NDE_Digital_Market.Model.DTO;

namespace NDE_Digital_Market.Services.ProductListService
{
    public interface IProductList_Service
    {
        Task<object> CreateProductGroupsAsync(InsertProductListDTO productDto);

        //======================================================================

        Task<object> UpdateProductListAsync(UpdateProductListDTO productDto);

        Task<List<GetAllProductListDTO>> GetProductGroupsListAsync();


        Task<List<GetProductListByStatusDTO>> GetProductListByStatus(bool? status = null);


        Task<List<GetAllProductListDTO>> GetProductNameByProductGroupId(string ProductGroupId);

        Task<object> MakeProductActiveOrInactiveAsync(List<string> productIds, bool? IsActive);

    }
}
