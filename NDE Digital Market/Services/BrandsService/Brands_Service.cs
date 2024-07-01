using NDE_Digital_Market.Data_Access_Layer;
using NDE_Digital_Market.Model;
using NDE_Digital_Market.Model.DTO;
using NDE_Digital_Market.SharedServices;
using System.Data;

namespace NDE_Digital_Market.Services.BrandsService
{
    public class Brands_Service : IBrands_Service
    {
        private readonly Brands_DAL _Brands_DAL;
        public Brands_Service(Brands_DAL Brands_DAL)
        {
            _Brands_DAL = Brands_DAL;
        }

        public async Task<List<GetBrandsDataDTO>> GetBrandListAsync(bool? isActive)
        {

            DataTable dataTable = await _Brands_DAL.GetBrandListAsync(isActive);

            List<GetBrandsDataDTO> list = new List<GetBrandsDataDTO>();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                GetBrandsDataDTO modelObj = new GetBrandsDataDTO();
                modelObj.BrandId = CommonServices.EncryptPassword(row["BrandId"].ToString());
                modelObj.BrandName = row["BrandName"].ToString();
                modelObj.Description = row["Description"].ToString() ?? "";
                modelObj.ShortName = row["ShortName"].ToString() ?? "";
                modelObj.IsActive = Convert.ToBoolean(row["IsActive"]);

                list.Add(modelObj);
            }

            return list;
        }



        public async Task<object> PostBrandAsync(InsertBrandDataDTO modelbrand)
        {
            BrandsModel Model = new BrandsModel();
            Model.BrandName = modelbrand.BrandName;
            Model.ShortName = modelbrand.ShortName;
            Model.Description = modelbrand.Description;
            Model.IsActive = modelbrand.IsActive;
            Model.AddedBy = modelbrand.AddedBy;
            Model.AddedPC = modelbrand.AddedPC;
            Model.AddedDate = DateTime.UtcNow;


            return await _Brands_DAL.PostBrandAsync(Model);
        }


        public async Task<object> PutBrand(UpdateBrandsDTO modelbrand)
        {
            BrandsModel Model = new BrandsModel();
            Model.BrandId = int.Parse(CommonServices.DecryptPassword(modelbrand.BrandId));
            Model.BrandName = modelbrand.BrandName;
            Model.ShortName = modelbrand.ShortName;
            Model.Description = modelbrand.Description;
            Model.IsActive = modelbrand.IsActive;
            Model.UpdatedBy = modelbrand.UpdatedBy;
            Model.UpdatedPC = modelbrand.UpdatedPC;
            Model.UpdatedDate = DateTime.UtcNow;


            return await _Brands_DAL.PutBrand(Model);
        }



        public async Task<object> ChangeBrandsStatus(string BrandIDs, bool isActive)
        {
            string[] ids = BrandIDs.Split(',');

            // Assuming each decrypted ID should be an int, and you want an array of decrypted int IDs.
            string[] decryptedIds = new string[ids.Length];

            for (int i = 0; i < ids.Length; i++)
            {
                decryptedIds[i] = CommonServices.DecryptPassword(ids[i]);
            }
            return await _Brands_DAL.ChangeBrandsStatus(decryptedIds, isActive);
        }

    }
}
