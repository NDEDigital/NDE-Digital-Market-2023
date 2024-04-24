using Microsoft.AspNetCore.Mvc;
using NDE_Digital_Market.Data_Access_Layer;
using NDE_Digital_Market.DTOs;
using NDE_Digital_Market.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NDE_Digital_Market.Services.AddBanner
{
    public class Addbanners : IAddBanner
    {
        private readonly AddBanner_DAL _AddBanner_DAL;

        public Addbanners(AddBanner_DAL banner_DAL)
        {
            this._AddBanner_DAL = banner_DAL;
        }

        public async Task<string> AddBanners(BannerDto bannerDto)
        {
            string res = await _AddBanner_DAL.AddBanners(bannerDto);
            return res;
        }
        public async Task<List<ImageBanner>> GetAddBanner()
        {
            var Banner = await _AddBanner_DAL.GetAddBanner();
            return Banner;
        }

     


        //public async Task<List<BannerDto>> GetBanners()
        //{
        //    return await _AddBanner_DAL.GetBanners();
        //}

        public async Task<bool> DeleteBanner(int bannerId)
        {
            return await _AddBanner_DAL.DeleteBanner(bannerId);
        }


        public async Task<string> UpdateBanner(BannerDto banner)
        {
            var mssg = await _AddBanner_DAL.UpdateBanner(banner);
            return mssg;
        }





    }
}
