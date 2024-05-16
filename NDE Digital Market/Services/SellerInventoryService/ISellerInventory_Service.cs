using NDE_Digital_Market.Model.DTO;

namespace NDE_Digital_Market.Services.SellerInventoryService
{
    public interface ISellerInventory_Service
    {
        Task<List<GetSellerInventoryInfoDTO>> GetSellerInventoryDataBySellerId(string UserId);
    }
}
