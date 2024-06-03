using NDE_Digital_Market.Data_Access_Layer;
using NDE_Digital_Market.Model;
using NDE_Digital_Market.Model.DTO;
using NDE_Digital_Market.SharedServices;
using System.Data;

namespace NDE_Digital_Market.Services.ProductListService;

public class ProductList_Service: IProductList_Service
{
    private readonly ProductList_DAL _productList_DAL;
    public ProductList_Service(ProductList_DAL productList_DAL)
    {
        _productList_DAL = productList_DAL;
    }

    public async Task<object> CreateProductGroupsAsync(InsertProductListDTO productDto)
    {
        ProductListModel Model = new ProductListModel();

        Model.ProductName = productDto.ProductName;
        Model.ProductGroupID = int.Parse(CommonServices.DecryptPassword(productDto.ProductGroupID));
        Model.Specification = productDto.Specification;
        Model.BrandId = int.Parse(CommonServices.DecryptPassword(productDto.BrandId));
        Model.UnitId = int.Parse(CommonServices.DecryptPassword(productDto.UnitId));
        Model.ImageFile = productDto.ImageFile;
        //Model.ExistingImageFileName = productDto.ExistingImageFileName;
        Model.ProductSubName = productDto.ProductSubName;
        Model.AddedBy = productDto.AddedBy;
        Model.AddedPC = productDto.AddedPC;

        return await _productList_DAL.CreateProductGroupsAsync(Model);
    }

    //======================================================================

    public async Task<object> UpdateProductListAsync(UpdateProductListDTO productDto)
    {
        ProductListModel Model = new ProductListModel();

        Model.ProductId = int.Parse(CommonServices.DecryptPassword(productDto.ProductId));
        Model.ProductName = productDto.ProductName;
        Model.ProductGroupID = int.Parse(CommonServices.DecryptPassword(productDto.ProductGroupID));
        Model.Specification = productDto.Specification;
        Model.BrandId = int.Parse(CommonServices.DecryptPassword(productDto.BrandId));
        Model.UnitId = int.Parse(CommonServices.DecryptPassword(productDto.UnitId));
        Model.ImageFile = productDto.ImageFile;
        Model.ExistingImageFileName = productDto.ExistingImageFileName;
        Model.ProductSubName = productDto.ProductSubName;
        Model.UpdatedBy = productDto.UpdatedBy;
        Model.UpdatedPC = productDto.UpdatedPC;

        return await _productList_DAL.UpdateProductListAsync(Model);
    }

    //======================================================================
    public async Task<List<GetAllProductListDTO>> GetProductGroupsListAsync()
    {

        DataTable dataTable = await _productList_DAL.GetProductGroupsListAsync();

        List<GetAllProductListDTO> list = new List<GetAllProductListDTO>();
        // Check if dataTable is null
        if (dataTable == null)
        {
            return null;
        }
        foreach (DataRow row in dataTable.Rows)
        {
            GetAllProductListDTO modelObj = new GetAllProductListDTO();

            modelObj.ProductId = CommonServices.EncryptPassword(row["ProductId"].ToString());
            modelObj.ProductName = row["ProductName"].ToString();
            modelObj.UnitId = CommonServices.EncryptPassword((row["UnitId"].ToString()));
            modelObj.UnitName = row["UnitName"].ToString();
            modelObj.ProductGroupId = CommonServices.EncryptPassword(row["ProductGroupID"].ToString());
            modelObj.ProductGroupName = row["ProductGroupName"].ToString();

            list.Add(modelObj);
        }
        return list;
    }


    public async Task<List<GetProductListByStatusDTO>> GetProductListByStatus(bool? status = null)
    {
        DataTable dataTable = await _productList_DAL.GetProductListByStatus(status);

        List<GetProductListByStatusDTO> list = new List<GetProductListByStatusDTO>();
        // Check if dataTable is null
        if (dataTable == null)
        {
            return null;
        }
        foreach (DataRow row in dataTable.Rows)
        {
            GetProductListByStatusDTO modelObj = new GetProductListByStatusDTO();

            modelObj.ProductId = CommonServices.EncryptPassword(row["ProductId"].ToString());
            modelObj.UnitId = CommonServices.EncryptPassword(row["UnitId"].ToString());
            modelObj.Unit = row["Unit"].ToString();
            modelObj.BrandId = row["BrandId"] == DBNull.Value ? null : CommonServices.EncryptPassword(row["BrandId"].ToString());
            modelObj.BrandName = row["BrandName"] == DBNull.Value ? String.Empty : row["BrandName"].ToString();
            modelObj.ProductGroupID = CommonServices.EncryptPassword(row["ProductGroupID"].ToString());
            modelObj.ProductGroupName = row["ProductGroupName"].ToString();
            modelObj.ProductName = row["ProductName"].ToString();
            modelObj.Specification = row["Specification"].ToString();
            modelObj.ImagePath = row["ImagePath"].ToString();
            modelObj.ProductSubName = row["ProductSubName"].ToString();
            modelObj.IsActive = row["IsActive"] is DBNull ? (bool?)null : (bool)row["IsActive"];
            modelObj.AddedDate = row["AddedDate"] is DBNull ? (DateTime?)null : (DateTime)row["AddedDate"];

            list.Add(modelObj);
        }
        return list;
    }




    // ==============================productName by productGroupId===================

    public async Task<List<GetAllProductListDTO>> GetProductNameByProductGroupId(string ProductGroupId)
    {
        int groupId = int.Parse(CommonServices.DecryptPassword(ProductGroupId));
        DataTable dataTable = await _productList_DAL.GetProductNameByProductGroupId(groupId);

        List<GetAllProductListDTO> list = new List<GetAllProductListDTO>();
        // Check if dataTable is null
        if (dataTable == null)
        {
            return null;
        }
        foreach (DataRow row in dataTable.Rows)
        {
            GetAllProductListDTO modelObj = new GetAllProductListDTO();

            modelObj.ProductId = CommonServices.EncryptPassword(row["ProductId"].ToString());
            modelObj.ProductName = row["ProductName"].ToString();
            modelObj.UnitName = row["UnitName"].ToString();
            modelObj.ProductGroupId = CommonServices.EncryptPassword(row["ProductGroupID"].ToString());
            modelObj.ProductGroupName = row["ProductGroupName"].ToString();

            list.Add(modelObj);
        }
        return list;
    }


    //========================tushar=========================
    public async Task<object> MakeProductActiveOrInactiveAsync(List<string> productIds, bool? IsActive)
    {

        // Assuming each decrypted ID should be an int, and you want an array of decrypted int IDs.
        List<int> decryptedIds = new List<int>();

        for (int i = 0; i < productIds.Count; i++)
        {
            //decryptedIds[i] = int.Parse(CommonServices.DecryptPassword(productIds[i]));
            decryptedIds.Add(int.Parse(CommonServices.DecryptPassword(productIds[i])));
        }
        return await _productList_DAL.MakeProductActiveOrInactiveAsync(decryptedIds, IsActive);
    }


}
