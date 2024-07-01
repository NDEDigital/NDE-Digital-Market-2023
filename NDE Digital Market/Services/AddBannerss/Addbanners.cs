using NDE_Digital_Market.Data_Access_Layer;
using NDE_Digital_Market.Model;
using NDE_Digital_Market.Model.DTO;
using NDE_Digital_Market.SharedServices;
using System.Data;

namespace NDE_Digital_Market.Services.AddBanner
{
    public class Addbanners : IAddBanner
    {
        private readonly AddBanner_DAL _AddBanner_DAL;

        public Addbanners(AddBanner_DAL banner_DAL)
        {
            this._AddBanner_DAL = banner_DAL;
        }

        public async Task<object> AddBanners(InsertAdsAndBannerDTO bannerDto)
        {
            AdsAndBannerModel Model = new AdsAndBannerModel();

            Model.UserId = int.Parse(CommonServices.DecryptPassword(bannerDto.UserId));
            Model.IsActive = bannerDto.IsActive;
            Model.IsAds = bannerDto.IsAds;
            Model.AddedBy = bannerDto.AddedBy;
            Model.AddedPC = bannerDto.AddedPC;
            Model.CompanyCode = CommonServices.DecryptPassword(bannerDto.CompanyCode);
            Model.BannerDescription = bannerDto.BannerDescription;
            Model.BannerImageFile = bannerDto.BannerImageFile;
            Model.StartDate = bannerDto.StartDate;
            Model.EndDate = bannerDto.EndDate;
            Model.IsPayment = bannerDto.IsPayment;
            Model.PaymentRemarks = bannerDto.PaymentRemarks;
            Model.IsBannerStatus = bannerDto.IsBannerStatus;

            return await _AddBanner_DAL.AddBanners(Model);
        }
        public async Task<List<GetAdsAndBannerForAdminByStatusDTO>> GetAddBannerForAdmin(bool? status)
        {
            DataTable dataTable = await _AddBanner_DAL.GetAddBannerForAdmin(status);

            List<GetAdsAndBannerForAdminByStatusDTO> list = new List<GetAdsAndBannerForAdminByStatusDTO>();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }
            foreach (DataRow row in dataTable.Rows)
            {
                GetAdsAndBannerForAdminByStatusDTO banner = new GetAdsAndBannerForAdminByStatusDTO();
                //banner.UserId = reader.GetInt32(0);

                banner.BannerID = CommonServices.EncryptPassword(row["BannerID"].ToString());
                banner.BannerDescription = row["BannerDescription"].ToString();
                banner.BannerImage = row["BannerImage"].ToString();
                banner.CompanyName = row["CompanyName"].ToString();


                //banner.StartDate =Convert.ToDateTime(reader["StartDate"]);
                //banner.EndDate =Convert.ToDateTime(reader["EndDate"]);

                if (!row.IsNull("IsAds"))
                {
                    banner.IsAds = (bool)row["IsAds"];
                }
                else
                {
                    // Handle the case when IsAds is DBNull
                    banner.IsAds = null;
                }
                if (!row.IsNull("AddedDate"))
                {
                    banner.AddedDate = Convert.ToDateTime(row["AddedDate"]);
                }
                else
                {
                    // Handle the case when StartDate is DBNull
                    banner.AddedDate = null;
                }
                if (!row.IsNull("StartDate"))
                {
                    banner.StartDate = Convert.ToDateTime(row["StartDate"]);
                }
                else
                {
                    // Handle the case when StartDate is DBNull
                    banner.StartDate = null;
                }
                if (!row.IsNull("EndDate"))
                {
                    banner.EndDate = Convert.ToDateTime(row["EndDate"]);
                }
                else
                {
                    // Handle the case when StartDate is DBNull
                    banner.EndDate = null;
                }

                list.Add(banner);
            }
            return list;
        }
        
        
        public async Task<List<GetAdsAndBannerForSellerByCompanyCodeDTO>> GetAddBannerForSeller(string ComapnayCode)
        {

            string decryptcompanycode = CommonServices.DecryptPassword(ComapnayCode);
            DataTable dataTable = await _AddBanner_DAL.GetAddBannerForSeller(decryptcompanycode);

            List<GetAdsAndBannerForSellerByCompanyCodeDTO> list = new List<GetAdsAndBannerForSellerByCompanyCodeDTO>();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }
            foreach (DataRow row in dataTable.Rows)
            {
                GetAdsAndBannerForSellerByCompanyCodeDTO banner = new GetAdsAndBannerForSellerByCompanyCodeDTO();

                banner.BannerID = CommonServices.EncryptPassword(row["BannerID"].ToString());
                banner.BannerDescription = row["BannerDescription"].ToString();
                banner.BannerImage = row["BannerImage"].ToString();
                banner.CompanyName = row["CompanyName"].ToString();

                banner.IsBannerStatus = row.IsNull("IsBannerStatus") ? (bool?)null : (bool)row["IsBannerStatus"];
                banner.IsAds = row.IsNull("IsAds") ? (bool?)null : (bool)row["IsAds"];
                banner.IsActive = (bool)row["IsActive"];

                if (!row.IsNull("AddedDate"))
                {
                    banner.AddedDate = (DateTime)row["AddedDate"];
                }

                list.Add(banner);
            }

            return list;
        }

        public async Task<List<GetBannerAndAdsForShowingInHomePageDTO>> GetBannerForShowingInHomePage()
        {

            DataTable dataTable = await _AddBanner_DAL.GetBannerForShowingInHomePage();

            List<GetBannerAndAdsForShowingInHomePageDTO> list = new List<GetBannerAndAdsForShowingInHomePageDTO>();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }
            foreach (DataRow row in dataTable.Rows)
            {
                GetBannerAndAdsForShowingInHomePageDTO banner = new GetBannerAndAdsForShowingInHomePageDTO();

                banner.BannerImage = row["BannerImage"].ToString();

                list.Add(banner);
            }

            return list;
        }


        public async Task<List<GetBannerAndAdsForShowingInHomePageDTO>> GetAddForShowingInHomePage()
        {
            DataTable dataTable = await _AddBanner_DAL.GetAddForShowingInHomePage();

            List<GetBannerAndAdsForShowingInHomePageDTO> list = new List<GetBannerAndAdsForShowingInHomePageDTO>();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }
            foreach (DataRow row in dataTable.Rows)
            {
                GetBannerAndAdsForShowingInHomePageDTO banner = new GetBannerAndAdsForShowingInHomePageDTO();

                banner.BannerImage = row["BannerImage"].ToString();

                list.Add(banner);
            }

            return list;
        }



        //public async Task<List<BannerDto>> GetBanners()
        //{
        //    return await _AddBanner_DAL.GetBanners();
        //}

        public async Task<object> DeleteBanner(string bannerId)
        {
            int decryptbannerId = int.Parse(CommonServices.DecryptPassword(bannerId));
            return await _AddBanner_DAL.DeleteBanner(decryptbannerId);
        }


        public async Task<object> UpdateBanner(UpdateAdsAndBannerDTO bannerDto)
        {
            AdsAndBannerModel Model = new AdsAndBannerModel();

            Model.BannerID = int.Parse(CommonServices.DecryptPassword(bannerDto.BannerID));
            Model.UserId = int.Parse(CommonServices.DecryptPassword(bannerDto.UserId));
            Model.IsActive = bannerDto.IsActive;
            Model.IsAds = bannerDto.IsAds;
            Model.UpdatedPC = bannerDto.UpdatedPC;
            Model.UpdatedBy = bannerDto.UpdatedBy;
            Model.CompanyCode = CommonServices.DecryptPassword(bannerDto.CompanyCode);
            Model.BannerDescription = bannerDto.BannerDescription;
            Model.BannerImageFile = bannerDto.BannerImageFile;
            Model.StartDate = bannerDto.StartDate;
            Model.EndDate = bannerDto.EndDate;
            Model.IsPayment = bannerDto.IsPayment;
            Model.PaymentRemarks = bannerDto.PaymentRemarks;
            Model.IsBannerStatus = bannerDto.IsBannerStatus;

            return await _AddBanner_DAL.UpdateBanner(Model);
        }


        public async Task<object> UpdateBannerStatus(UpdateAdsAndBannerDTO bannerDto)
        {
            AdsAndBannerModel Model = new AdsAndBannerModel();

            Model.BannerID = bannerDto.BannerID != null ? int.Parse(CommonServices.DecryptPassword(bannerDto.BannerID)) : (int?)null;
            Model.UserId = bannerDto.UserId != null ? int.Parse(CommonServices.DecryptPassword(bannerDto.UserId)) : (int?)null;
            Model.IsActive = bannerDto.IsActive;
            Model.IsAds = bannerDto.IsAds;
            Model.UpdatedPC = bannerDto.UpdatedPC;
            Model.UpdatedBy = bannerDto.UpdatedBy;
            Model.CompanyCode = bannerDto.CompanyCode != null ? CommonServices.DecryptPassword(bannerDto.CompanyCode) : null;
            Model.BannerDescription = bannerDto.BannerDescription;
            Model.BannerImageFile = bannerDto.BannerImageFile;
            Model.StartDate = bannerDto.StartDate;
            Model.EndDate = bannerDto.EndDate;
            Model.IsPayment = bannerDto.IsPayment;
            Model.PaymentRemarks = bannerDto.PaymentRemarks;
            Model.IsBannerStatus = bannerDto.IsBannerStatus;



            return await _AddBanner_DAL.UpdateBanner(Model);
        }


        //public async Task<int> UpdateBanners(ImageBanner banner)
        //{
        //    var upd = await _AddBanner_DAL.UpdateBanners(banner);
        //    return upd;

        //}


    }
}
