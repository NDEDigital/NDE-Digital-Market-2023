
using NDE_Digital_Market.Model.DTO;

namespace NDE_Digital_Market.Services.AddBanner

{
    public interface IAddBanner
    {
        Task<object> AddBanners(InsertAdsAndBannerDTO bannerDto);
        Task<List<GetAdsAndBannerForAdminByStatusDTO>> GetAddBannerForAdmin(bool? status);


        Task<List<GetAdsAndBannerForSellerByCompanyCodeDTO>> GetAddBannerForSeller(string ComapnayCode);

        Task<List<GetBannerAndAdsForShowingInHomePageDTO>> GetBannerForShowingInHomePage();


        Task<List<GetBannerAndAdsForShowingInHomePageDTO>> GetAddForShowingInHomePage();

        Task<object> DeleteBanner(string bannerId);


        Task<object> UpdateBanner(UpdateAdsAndBannerDTO bannerDto);

        Task<object> UpdateBannerStatus(UpdateAdsAndBannerDTO bannerDto);
        //Task<List<BannerDto>> GetBanners();

        //Task<ImageBanner> GetBannerById(int bannerId);


        //Task<int> UpdateBanners(ImageBanner banner);


    }
}
