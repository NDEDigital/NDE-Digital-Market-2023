using NDE_Digital_Market.Data_Access_Layer;
using NDE_Digital_Market.Model;
using NDE_Digital_Market.Model.DTO;
using NDE_Digital_Market.SharedServices;
using System.Data;

namespace NDE_Digital_Market.Services.ProductQuantityService
{
    public class ProductQuantity_Service : IProductQuantity_Service
    {
        private readonly ProductQuantity_DAL _productQuantity_DAL;
        public ProductQuantity_Service(ProductQuantity_DAL productQuantity_DAL)
        {
            _productQuantity_DAL = productQuantity_DAL;
        }



        public async Task<List<ProductGroupsListForDropdownDTO>> ProductGroupsDropdownByUserId(string userID)
        {

            int decryptId = int.Parse(CommonServices.DecryptPassword(userID));

            DataTable dataTable = await _productQuantity_DAL.ProductGroupsDropdownByUserId(decryptId);

            List<ProductGroupsListForDropdownDTO> list = new List<ProductGroupsListForDropdownDTO>();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }
            foreach (DataRow row in dataTable.Rows)
            {
                ProductGroupsListForDropdownDTO modelObj = new ProductGroupsListForDropdownDTO();

                modelObj.ProductGroupID = CommonServices.EncryptPassword(row["ProductGroupID"].ToString());
                modelObj.ProductGroupName = row["ProductGroupName"].ToString();

                list.Add(modelObj);
            }
            return list;
        }


        public async Task<List<GetSellerProductListForAddQtyDTO>> GetProductForAddQtyByUserId(string UserId, string productGroupId)
        {

            int DecryptUserId = int.Parse(CommonServices.DecryptPassword(UserId));
            int DecryptGroupId = int.Parse(CommonServices.DecryptPassword(productGroupId));

            DataTable dataTable = await _productQuantity_DAL.GetProductForAddQtyByUserId(DecryptUserId, DecryptGroupId);

            List<GetSellerProductListForAddQtyDTO> list = new List<GetSellerProductListForAddQtyDTO>();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }
            foreach (DataRow row in dataTable.Rows)
            {
                GetSellerProductListForAddQtyDTO modelObj = new GetSellerProductListForAddQtyDTO();

                modelObj.ProductId = CommonServices.EncryptPassword(row["ProductId"].ToString());
                modelObj.ProductName = row["ProductName"].ToString();
                modelObj.ProductGroupId = CommonServices.EncryptPassword(row["ProductGroupID"].ToString());
                modelObj.Specification = row["Specification"].ToString();
                modelObj.UnitId = CommonServices.EncryptPassword(row["UnitId"].ToString());
                modelObj.Unit = row["Unit"].ToString();
                modelObj.Price = Convert.ToDecimal(row["Price"]);
                modelObj.AvailableQty = Convert.ToDecimal(row["AvailableQty"]);

                list.Add(modelObj);
            }
            return list;

        }


        public async Task<object> InsertPortalReceivedAsync(InsertPortalReceivedMasterDTO portaldata)
        {

            PortalReceivedMasterModel Model = new PortalReceivedMasterModel();

            Model.MaterialReceivedDate = portaldata.MaterialReceivedDate;
            Model.ChallanNo = portaldata.ChallanNo;
            Model.ChallanDate = portaldata.ChallanDate;
            Model.Remarks = portaldata.Remarks;
            Model.UserId = int.Parse(CommonServices.DecryptPassword(portaldata.UserId));
            Model.CompanyCode = CommonServices.DecryptPassword(portaldata.CompanyCode);
            Model.AddedBy = portaldata.AddedBy;
            Model.AddedPC = portaldata.AddedPC;

            for(int i=0; i < portaldata.PortalReceivedDetailslist.Count; i++)
            {

                Model.PortalReceivedDetailslist[i].ProductGroupId = int.Parse(CommonServices.DecryptPassword(portaldata.PortalReceivedDetailslist[i].ProductGroupId));
                Model.PortalReceivedDetailslist[i].ProductId = int.Parse(CommonServices.DecryptPassword(portaldata.PortalReceivedDetailslist[i].ProductId));
                Model.PortalReceivedDetailslist[i].Specification = portaldata.PortalReceivedDetailslist[i].Specification;
                Model.PortalReceivedDetailslist[i].ReceivedQty = portaldata.PortalReceivedDetailslist[i].ReceivedQty;
                Model.PortalReceivedDetailslist[i].UnitId = int.Parse(CommonServices.DecryptPassword(portaldata.PortalReceivedDetailslist[i].UnitId));
                Model.PortalReceivedDetailslist[i].Price = portaldata.PortalReceivedDetailslist[i].Price;
                Model.PortalReceivedDetailslist[i].TotalPrice = portaldata.PortalReceivedDetailslist[i].TotalPrice;
                Model.PortalReceivedDetailslist[i].UserId = int.Parse(CommonServices.DecryptPassword(portaldata.PortalReceivedDetailslist[i].UserId));
                Model.PortalReceivedDetailslist[i].Remarks = portaldata.PortalReceivedDetailslist[i].Remarks;
                Model.PortalReceivedDetailslist[i].AddedBy = portaldata.PortalReceivedDetailslist[i].AddedBy;
                Model.PortalReceivedDetailslist[i].AddedPC = portaldata.PortalReceivedDetailslist[i].AddedPC;
            }


            return await _productQuantity_DAL.InsertPortalReceivedAsync(Model);
        }



        public async Task<object> CreateSellerProductPriceAndOfferAsync(InsertSellerProductPriceAndOfferDTO sellerproductdata)
        {
            SellerProductPriceAndOfferModel Model = new SellerProductPriceAndOfferModel();

            Model.ProductId = int.Parse(CommonServices.DecryptPassword(sellerproductdata.ProductId));
            Model.UserId = int.Parse(CommonServices.DecryptPassword(sellerproductdata.UserId));
            Model.Price = sellerproductdata.Price;
            Model.DiscountAmount = sellerproductdata.DiscountAmount;
            Model.DiscountPct = sellerproductdata.DiscountPct;
            Model.EffectivateDate = sellerproductdata.EffectivateDate;
            Model.EndDate = sellerproductdata.EndDate;
            Model.ImageFile = sellerproductdata.ImageFile;
            Model.TotalPrice = sellerproductdata.TotalPrice;
            Model.CompanyCode = CommonServices.DecryptPassword(sellerproductdata.CompanyCode);
            Model.AddedBy = sellerproductdata.AddedBy;
            Model.AddedPC = sellerproductdata.AddedPC;



            return await _productQuantity_DAL.CreateSellerProductPriceAndOfferAsync(Model);
        }


        public async Task<object> UpdateSellerProductPriceAndOffer(UpdateSellerProductPriceAndOfferDTO sellerproductdata)
        {
            SellerProductPriceAndOfferModel Model = new SellerProductPriceAndOfferModel();

            Model.ProductId = int.Parse(CommonServices.DecryptPassword(sellerproductdata.ProductId));
            Model.UserId = int.Parse(CommonServices.DecryptPassword(sellerproductdata.UserId));
            Model.Price = sellerproductdata.Price;
            Model.DiscountAmount = sellerproductdata.DiscountAmount;
            Model.DiscountPct = sellerproductdata.DiscountPct;
            Model.EffectivateDate = sellerproductdata.EffectivateDate;
            Model.EndDate = sellerproductdata.EndDate;
            Model.ImageFile = sellerproductdata.ImageFile;
            Model.TotalPrice = sellerproductdata.TotalPrice;
            Model.CompanyCode = CommonServices.DecryptPassword(sellerproductdata.CompanyCode);

            Model.UpdatedPC = sellerproductdata.UpdatedPC;
            Model.UpdatedBy = sellerproductdata.UpdatedBy;


            return await _productQuantity_DAL.UpdateSellerProductPriceAndOffer(Model);
        }


        public async Task<List<GetSellerProductForPriceAndOfferByUserIdDTO>> GetSellerProductsForPriceAndOfferByUserId(string UserId, Int32? status = null)
        {
            int DecryptUserId = int.Parse(CommonServices.DecryptPassword(UserId));

            DataTable dataTable = await _productQuantity_DAL.GetSellerProductsForPriceAndOfferByUserId(DecryptUserId, status);

            List<GetSellerProductForPriceAndOfferByUserIdDTO> list = new List<GetSellerProductForPriceAndOfferByUserIdDTO>();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                GetSellerProductForPriceAndOfferByUserIdDTO sellerProduct = new GetSellerProductForPriceAndOfferByUserIdDTO();

                sellerProduct.SellerProductId = CommonServices.EncryptPassword(row["SellerProductId"].ToString());
                sellerProduct.ProductId = CommonServices.EncryptPassword(row["ProductId"].ToString());
                sellerProduct.ProductName = row["ProductName"].ToString();
                sellerProduct.UserId = CommonServices.EncryptPassword(row["UserId"].ToString());
                sellerProduct.FullName = row["FullName"].ToString();
                sellerProduct.Price = row["Price"] != DBNull.Value ? Convert.ToDecimal(row["Price"]) : 0;
                sellerProduct.DiscountAmount = row["DiscountAmount"] != DBNull.Value ? Convert.ToDecimal(row["DiscountAmount"]) : 0;
                sellerProduct.DiscountPct = row["DiscountPct"] != DBNull.Value ? Convert.ToDecimal(row["DiscountPct"]) : 0;
                sellerProduct.EffectivateDate = row["EffectivateDate"] != DBNull.Value ? Convert.ToDateTime(row["EffectivateDate"]) : DateTime.MinValue;
                sellerProduct.EndDate = row["EndDate"] != DBNull.Value ? Convert.ToDateTime(row["EndDate"]) : DateTime.MinValue;
                sellerProduct.ImagePath = row["ImagePath"].ToString();
                sellerProduct.Status = row["Status"].ToString();
                sellerProduct.IsActive = row["IsActive"] != DBNull.Value ? Convert.ToBoolean(row["IsActive"]) : false;
                sellerProduct.TotalPrice = row["TotalPrice"] != DBNull.Value ? Convert.ToDecimal(row["TotalPrice"]) : 0;
                sellerProduct.AddedDate = row["AddedDate"] != DBNull.Value ? Convert.ToDateTime(row["AddedDate"]) : DateTime.MinValue;
                sellerProduct.UnitName = row["UnitName"].ToString();
                sellerProduct.ProductGroupName = row["ProductGroupName"].ToString();
                sellerProduct.ProductGroupID = CommonServices.EncryptPassword(row["ProductGroupID"].ToString());


                DateTime? endDate = null;
                if (row["EndDate"] != DBNull.Value)
                {
                    endDate = Convert.ToDateTime(row["EndDate"]);
                    if (endDate <= DateTime.UtcNow)
                    {
                        sellerProduct.TotalPrice = sellerProduct.Price;
                        sellerProduct.DiscountAmount = 0;
                        sellerProduct.DiscountPct = 0;
                    }
                }




                list.Add(sellerProduct);
            }
            return list;


        }


        public async Task<List<GetPortalReceivedListByUserIdDTO>> GetPortalReceivedByUserId(string UserId)
        {
            int DecryptUserId = int.Parse(CommonServices.DecryptPassword(UserId));
            DataTable dataTable = await _productQuantity_DAL.GetPortalReceivedByUserId(DecryptUserId);

            List<GetPortalReceivedListByUserIdDTO> list = new List<GetPortalReceivedListByUserIdDTO>();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                GetPortalReceivedListByUserIdDTO obj = new GetPortalReceivedListByUserIdDTO();
                {
                    obj.PortalReceivedId = CommonServices.EncryptPassword(row["PortalReceivedId"].ToString());
                    obj.PortalReceivedCode = CommonServices.EncryptPassword(row["PortalReceivedCode"].ToString());
                    obj.MaterialReceivedDate = row["MaterialReceivedDate"] != DBNull.Value ? Convert.ToDateTime(row["MaterialReceivedDate"]) : (DateTime?)null;
                    obj.UserId = CommonServices.EncryptPassword(row["UserId"].ToString());
                };
                list.Add(obj);
            }
            return list;

        }


        public async Task<GetPortalReceivedMasterDataAfterInsertDTO> GetPortalData(string PortalReceivedId)
        {
            int DecryptPortalReceivedId = int.Parse(CommonServices.DecryptPassword(PortalReceivedId));
            DataSet dataSet = await _productQuantity_DAL.GetPortalData(DecryptPortalReceivedId);

            GetPortalReceivedMasterDataAfterInsertDTO portalAfterInsert = new GetPortalReceivedMasterDataAfterInsertDTO();

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                // Processing the first result set (PortalReceivedMaster)
                DataTable masterTable = dataSet.Tables[0];
                if (masterTable.Rows.Count > 0)
                {
                    DataRow row = masterTable.Rows[0];
                    portalAfterInsert.PortalReceivedId = CommonServices.EncryptPassword(row["PortalReceivedId"].ToString());
                    portalAfterInsert.PortalReceivedCode = CommonServices.EncryptPassword(row["PortalReceivedCode"].ToString());
                    portalAfterInsert.MaterialReceivedDate = Convert.ToDateTime(row["MaterialReceivedDate"].ToString());
                    portalAfterInsert.ChallanNo = row["ChallanNo"].ToString();
                    portalAfterInsert.ChallanDate = row["ChallanDate"] != DBNull.Value ? Convert.ToDateTime(row["ChallanDate"]) : (DateTime?)null;
                    portalAfterInsert.Remarks = row["Remarks"].ToString();
                }

                // Processing the second result set (PortalReceivedDetails)
                DataTable detailsTable = dataSet.Tables[1];
                foreach (DataRow row in detailsTable.Rows)
                {
                    GetPortalReceivedDetailsDataAfterInsertDTO portalReceivedDetailAfterInsert = new GetPortalReceivedDetailsDataAfterInsertDTO();
                    portalReceivedDetailAfterInsert.PortalReceivedId = CommonServices.EncryptPassword(row["PortalReceivedId"].ToString());
                    portalReceivedDetailAfterInsert.PortalDetailsId = CommonServices.EncryptPassword(row["PortalDetailsId"].ToString());
                    portalReceivedDetailAfterInsert.ProductGroupId = CommonServices.EncryptPassword(row["ProductGroupId"].ToString());
                    portalReceivedDetailAfterInsert.ProductGroupName = row["ProductGroupName"].ToString();
                    portalReceivedDetailAfterInsert.ProductId = CommonServices.EncryptPassword(row["ProductId"].ToString());
                    portalReceivedDetailAfterInsert.ProductName = row["ProductName"].ToString();
                    portalReceivedDetailAfterInsert.Specification = row["Specification"].ToString();
                    portalReceivedDetailAfterInsert.ReceivedQty = Convert.ToDecimal(row["ReceivedQty"].ToString());
                    portalReceivedDetailAfterInsert.UnitId = CommonServices.EncryptPassword(row["UnitId"].ToString());
                    portalReceivedDetailAfterInsert.Unit = row["Unit"].ToString();
                    portalReceivedDetailAfterInsert.Price = Convert.ToDecimal(row["Price"].ToString());
                    portalReceivedDetailAfterInsert.TotalPrice = Convert.ToDecimal(row["TotalPrice"].ToString());
                    portalReceivedDetailAfterInsert.AvailableQty = Convert.ToDecimal(row["AvailableQty"].ToString());
                    portalReceivedDetailAfterInsert.Remarks = row["Remarks"].ToString();

                    portalAfterInsert.PortalReceivedDetailAfterInsertlList.Add(portalReceivedDetailAfterInsert);
                }
            }

            return portalAfterInsert;
        }


    }
}
