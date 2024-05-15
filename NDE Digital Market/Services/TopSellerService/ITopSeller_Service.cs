using NDE_Digital_Market.Model.DTO;

namespace NDE_Digital_Market.Services.TopSellerService
{
    public interface ITopSeller_Service
    {
        Task<List<TopSellerListDTO>> getForDropDown();
    }
}
