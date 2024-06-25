using NDE_Digital_Market.Model.DTO;

namespace NDE_Digital_Market.Services.GoodsService
{
    public interface IGoods_Service
    {
        Task<List<GetNavDataDTO>> GetNavData();

        Task<List<GetNavDataDTO>> getForDropDown();

        Task<List<GetAllProductListWithAvailableQty>> GetGoodsList();

        Task<GetAllProductListWithAvailableQty> GetGoodsDetails(string CompanyCode, string ProductId);

        Task<List<GetCompanyListDTO>> GetProductCompany(string ProductGroupCode);

        Task<List<GetCompanyWiseProductListDTO>> GetProductList(string? CompanyCode, string ProductGroupCode);

        Task<List<GetRecommendedProductListDTO>> GetRecommendedProductList(string CompanyCode, string ProductId);
    }
}
