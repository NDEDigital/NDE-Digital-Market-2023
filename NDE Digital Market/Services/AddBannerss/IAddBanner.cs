using NDE_Digital_Market.Model;
using NDE_Digital_Market.DTOs;
using NDE_Digital_Market.Data_Access_Layer;
using Microsoft.AspNetCore.Mvc;

namespace NDE_Digital_Market.Services.AddBanner

{
    public interface IAddBanner
    {
        Task<string> AddBanners(BannerDto bannerDto);

        Task<List<ImageBanner>> GetAddBannerForSeller(string ComapnayCode);
        Task<List<ImageBanner>> GetAddBannerForAdmin(bool? status);

        //Task<List<BannerDto>> GetBanners();

        //Task<ImageBanner> GetBannerById(int bannerId);

        Task<string> UpdateBanner(BannerDto Banner);



        Task<bool> DeleteBanner(int bannerId);
    }
}
