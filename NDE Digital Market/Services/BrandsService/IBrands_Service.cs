using NDE_Digital_Market.Model.DTO;

namespace NDE_Digital_Market.Services.BrandsService
{
    public interface IBrands_Service
    {
        Task<List<GetBrandsDataDTO>> GetBrandListAsync(bool? isActive);


        Task<object> PostBrandAsync(InsertBrandDataDTO modelbrand);


        Task<object> PutBrand(UpdateBrandsDTO modelbrand);



        Task<object> ChangeBrandsStatus(string BrandIDs, bool isActive);
    }
}
