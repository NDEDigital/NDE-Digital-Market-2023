using NDE_Digital_Market.Data_Access_Layer;
using NDE_Digital_Market.Model;
using NDE_Digital_Market.Model.DTO;
using NDE_Digital_Market.SharedServices;
using System.Data;

namespace NDE_Digital_Market.Services.ProductGroupService
{
    public class ProductGroup_Service : IProductGroup_Service
    {
        private readonly ProductGroup_DAL _productGroup_DAL;
        public ProductGroup_Service(ProductGroup_DAL productGroup_DAL)
        {
            _productGroup_DAL = productGroup_DAL;
        }


        public async Task<object> CreateProductGroupsAsync(InsertProductGroupDTO productGroupsDto)
        {
            ProductGroupModel Model = new ProductGroupModel();

            Model.ProductGroupName = productGroupsDto.ProductGroupName;
            Model.ProductGroupPrefix = productGroupsDto.ProductGroupPrefix;
            Model.ProductGroupDetails = productGroupsDto.ProductGroupDetails;
            Model.ImageFile = productGroupsDto.ImageFile;
            Model.AddedBy = productGroupsDto.AddedBy;
            Model.AddedPC = productGroupsDto.AddedPC;


            return await _productGroup_DAL.CreateProductGroupsAsync(Model);

        }

        ///========================================================================================

        public async Task<object> UpdateProductGroupsAsync(UpdateProductGroupDTO productGroupsDto)
        {
            ProductGroupModel Model = new ProductGroupModel();

            Model.ProductGroupID = int.Parse(CommonServices.DecryptPassword(productGroupsDto.ProductGroupID));
            Model.ProductGroupName = productGroupsDto.ProductGroupName;
            Model.ProductGroupPrefix = productGroupsDto.ProductGroupPrefix;
            Model.ProductGroupDetails = productGroupsDto.ProductGroupDetails;
            Model.ImageFile = productGroupsDto.ImageFile;
            Model.ExistingImageFileName = productGroupsDto.ExistingImageFileName;
            Model.UpdatedBy = CommonServices.DecryptPassword(productGroupsDto.UpdatedBy);
            Model.UpdatedPC = productGroupsDto.UpdatedPC;

            return await _productGroup_DAL.UpdateProductGroupsAsync(Model);
        }

        /// =====================================================================



        public async Task<List<GetAllProductGroupDTO>> GetProductGroupsListAsync()
        {
            DataTable dataTable = await _productGroup_DAL.GetProductGroupsListAsync();

            List<GetAllProductGroupDTO> list = new List<GetAllProductGroupDTO>();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }
            foreach (DataRow row in dataTable.Rows)
            {
                GetAllProductGroupDTO modelObj = new GetAllProductGroupDTO();

                modelObj.ProductGroupID = CommonServices.EncryptPassword(row["ProductGroupID"].ToString());
                modelObj.ProductGroupCode = CommonServices.EncryptPassword(row["ProductGroupCode"].ToString());
                modelObj.ProductGroupName = row["ProductGroupName"].ToString();
                modelObj.ProductGroupPrefix = row["ProductGroupPrefix"].ToString();
                modelObj.ProductGroupDetails = row["ProductGroupDetails"].ToString();
                modelObj.IsActive = Convert.ToBoolean(row["IsActive"]);

                list.Add(modelObj);
            }
            return list;
        }




        public async Task<List<GetProductGroupListByStatusDTO>> GetProductGroupsListByStatus(Int32? status = null)
        {
            DataTable dataTable = await _productGroup_DAL.GetProductGroupsListByStatus(status);

            List<GetProductGroupListByStatusDTO> list = new List<GetProductGroupListByStatusDTO>();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }
            foreach (DataRow row in dataTable.Rows)
            {
                GetProductGroupListByStatusDTO modelObj = new GetProductGroupListByStatusDTO();

                modelObj.ProductGroupID = CommonServices.EncryptPassword(row["ProductGroupID"].ToString());
                modelObj.ProductGroupCode = CommonServices.EncryptPassword(row["ProductGroupCode"].ToString());
                modelObj.ProductGroupName = row["ProductGroupName"].ToString();
                modelObj.ProductGroupPrefix = row["ProductGroupPrefix"].ToString();
                modelObj.ProductGroupDetails = row["ProductGroupDetails"].ToString();
                modelObj.IsActive = Convert.ToBoolean(row["IsActive"]);
                modelObj.Imagepath = row["Imagepath"].ToString();
                modelObj.DateAdded = row.IsNull("DateAdded") ? (DateTime?)null : (DateTime?)row["DateAdded"];

                list.Add(modelObj);
            }
            return list;
        }


        //========================tushar=========================


        public async Task<object> MakeGroupActiveOrInactiveAsync(string groupIds, bool? IsActive)
        {
            List<string> decryptedIds = groupIds.Split(',').ToList();
            List<string> decryptgroupIdsList = new List<string>();

            for (int i = 0; i < decryptedIds.Count; i++)
            {
                decryptgroupIdsList.Add(CommonServices.DecryptPassword(decryptedIds[i]));
            }

            string decryptgroupIds = string.Join(",", decryptgroupIdsList);

            return await _productGroup_DAL.MakeGroupActiveOrInactiveAsync(decryptgroupIds, IsActive);
        }


    }
}
