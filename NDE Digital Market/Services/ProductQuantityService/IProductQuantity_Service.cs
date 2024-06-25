using NDE_Digital_Market.Model.DTO;

namespace NDE_Digital_Market.Services.ProductQuantityService
{
    public interface IProductQuantity_Service
    {

        Task<List<ProductGroupsListForDropdownDTO>> ProductGroupsDropdownByUserId(string userID);


        Task<List<GetSellerProductListForAddQtyDTO>> GetProductForAddQtyByUserId(string UserId, string productGroupId);


        Task<object> InsertPortalReceivedAsync(InsertPortalReceivedMasterDTO portaldata);



        Task<object> CreateSellerProductPriceAndOfferAsync(InsertSellerProductPriceAndOfferDTO sellerproductdata);


        Task<object> UpdateSellerProductPriceAndOffer(UpdateSellerProductPriceAndOfferDTO sellerproductdata);


        Task<List<GetSellerProductForPriceAndOfferByUserIdDTO>> GetSellerProductsForPriceAndOfferByUserId(string UserId, Int32? status = null);


        Task<List<GetPortalReceivedListByUserIdDTO>> GetPortalReceivedByUserId(string UserId);


        Task<GetPortalReceivedMasterDataAfterInsertDTO> GetPortalData(string PortalReceivedId);

    }
}
