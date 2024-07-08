using NDE_Digital_Market.Data_Access_Layer;
using NDE_Digital_Market.Model.DTO;
using NDE_Digital_Market.SharedServices;
using System.Data;

namespace NDE_Digital_Market.Services.GoodsService
{
    public class Goods_Service : IGoods_Service
    {

        private readonly Goods_DAL _Goods_DAL;
        public Goods_Service(Goods_DAL Goods_DAL)
        {
            _Goods_DAL = Goods_DAL;
        }


        public async Task<List<GetNavDataDTO>> GetNavData()
        {

            DataTable dataTable = await _Goods_DAL.GetNavData();

            List<GetNavDataDTO> list = new List<GetNavDataDTO>();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                GetNavDataDTO modelObj = new GetNavDataDTO
                {
                    ProductGroupCode = row["ProductGroupCode"].ToString(),
                    ProductGroupName = row["ProductGroupName"].ToString(),
                    //ProductGroupPrefix = reader["ProductGroupPrefix"].ToString(),
                    //ProductGroupDetails = reader["ProductGroupDetails"].ToString(),
                    ImagePath = row["ImagePath"].ToString(),
                    ProductGroupID = CommonServices.EncryptPassword(row["ProductGroupID"].ToString())
                };
                list.Add(modelObj);
            }

            return list;
        }



        public async Task<List<GetNavDataDTO>> getForDropDown()
        {
            
            DataTable dataTable = await _Goods_DAL.getForDropDown();

            List<GetNavDataDTO> list = new List<GetNavDataDTO>();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                GetNavDataDTO modelObj = new GetNavDataDTO
                {
                    ProductGroupCode = row["ProductGroupCode"].ToString(),
                    ProductGroupName = row["ProductGroupName"].ToString(),
                    ProductGroupPrefix = row["ProductGroupPrefix"].ToString(),
                    ProductGroupDetails = row["ProductGroupDetails"].ToString(),
                    ImagePath = row["ImagePath"].ToString(),
                    ProductGroupID = CommonServices.EncryptPassword(row["ProductGroupID"].ToString())
                };
                list.Add(modelObj);
            }

            return list;
        }



        public async Task<List<GetAllProductListWithAvailableQty>> GetGoodsList()
        {

            DataTable dataTable = await _Goods_DAL.GetGoodsList();

            List<GetAllProductListWithAvailableQty> list = new List<GetAllProductListWithAvailableQty>();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                GetAllProductListWithAvailableQty obj = new GetAllProductListWithAvailableQty();

                obj.CompanyCode = row["CompanyCode"].ToString();
                obj.CompanyName = row["CompanyName"].ToString();
                obj.ProductGroupName = row["ProductGroupName"].ToString();
                obj.ProductId = CommonServices.EncryptPassword(row["ProductId"].ToString());
                obj.ProductName = row["ProductName"].ToString();
                obj.ProductGroupID = CommonServices.EncryptPassword(row["ProductGroupID"].ToString());
                obj.ProductGroupCode = row["ProductGroupCode"].ToString();
                obj.Specification = row["Specification"].ToString();
                obj.UnitId = CommonServices.EncryptPassword(row["UnitId"].ToString());
                obj.Unit = row["Unit"].ToString();
                obj.Price = row["Price"] != DBNull.Value ? Convert.ToDecimal(row["Price"]) : 0;
                obj.DiscountAmount = row["DiscountAmount"] != DBNull.Value ? Convert.ToDecimal(row["DiscountAmount"]) : 0;
                obj.DiscountPct = row["DiscountPct"] != DBNull.Value ? Convert.ToDecimal(row["DiscountPct"]) : 0;
                obj.ImagePath = row["ImagePath"].ToString();
                obj.TotalPrice = row["TotalPrice"] != DBNull.Value ? Convert.ToDecimal(row["TotalPrice"]) : 0;
                obj.SellerId = CommonServices.EncryptPassword(row["SellerId"].ToString());
                obj.AvailableQty = Convert.ToInt32(row["AvailableQty"]);
                DateTime? endDate = null;
                if (row["EndDate"] != DBNull.Value)
                {
                    endDate = Convert.ToDateTime(row["EndDate"]);
                    if (endDate <= DateTime.Now)
                    {

                        obj.TotalPrice = obj.Price;
                        obj.DiscountAmount = 0;
                        obj.DiscountPct = 0;
                    }
                }


                list.Add(obj);
            }

            return list;
        }




        public async Task<GetAllProductListWithAvailableQty> GetGoodsDetails(string CompanyCode, string ProductId)
        {
            int decryptProductId = int.Parse(CommonServices.DecryptPassword(ProductId));
            //string decryptCompanyCode = CommonServices.DecryptPassword(CompanyCode);
            DataTable dataTable = await _Goods_DAL.GetGoodsDetails(CompanyCode, decryptProductId);

            GetAllProductListWithAvailableQty obj = new GetAllProductListWithAvailableQty();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }

            foreach (DataRow row in dataTable.Rows)
            {

                obj.CompanyCode = row["CompanyCode"].ToString();
                obj.CompanyName = row["CompanyName"].ToString();
                obj.ProductGroupName = row["ProductGroupName"].ToString();
                obj.ProductId = CommonServices.EncryptPassword(row["ProductId"].ToString());
                obj.ProductName = row["ProductName"].ToString();
                obj.ProductGroupID = CommonServices.EncryptPassword(row["ProductGroupID"].ToString());
                obj.Specification = row["Specification"].ToString();
                obj.UnitId = CommonServices.EncryptPassword(row["UnitId"].ToString());
                obj.Unit = row["Unit"].ToString();
                obj.Price = row["Price"] != DBNull.Value ? Convert.ToDecimal(row["Price"]) : 0;
                obj.DiscountAmount = row["DiscountAmount"] != DBNull.Value ? Convert.ToDecimal(row["DiscountAmount"]) : 0;
                obj.DiscountPct = row["DiscountPct"] != DBNull.Value ? Convert.ToDecimal(row["DiscountPct"]) : 0;
                obj.ImagePath = row["ImagePath"].ToString();
                obj.TotalPrice = row["TotalPrice"] != DBNull.Value ? Convert.ToDecimal(row["TotalPrice"]) : 0;
                obj.SellerId = CommonServices.EncryptPassword(row["SellerId"].ToString());
                obj.AvailableQty = Convert.ToInt32(row["AvailableQty"]);
                DateTime? endDate = null;
                if (row["EndDate"] != DBNull.Value)
                {
                    endDate = Convert.ToDateTime(row["EndDate"]);
                    if (endDate <= DateTime.Now)
                    {

                        obj.TotalPrice = obj.Price;
                        obj.DiscountAmount = 0;
                        obj.DiscountPct = 0;
                    }
                }

            }
            return obj;
        }



        public async Task<List<GetCompanyListDTO>> GetProductCompany(string ProductGroupCode)
        {
            //string decryptProductGroupCode = CommonServices.DecryptPassword(ProductGroupCode);
            DataTable dataTable = await _Goods_DAL.GetProductCompany(ProductGroupCode);

            List<GetCompanyListDTO> list = new List<GetCompanyListDTO>();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                var companiesByProduct = new GetCompanyListDTO
                {
                    CompanyName = row["CompanyName"].ToString(),
                    CompanyCode = row["CompanyCode"].ToString(),
                    CompanyImage = row["CompanyImage"].ToString()
                };
                list.Add(companiesByProduct);
            }

            return list;
        }



        public async Task<List<GetCompanyWiseProductListDTO>> GetProductList(string? CompanyCode, string? ProductGroupCode)
        {
            //string decryptCompanyCode = null;
            //if (CompanyCode != null)
            //{
            //    decryptCompanyCode = CommonServices.DecryptPassword(CompanyCode);
            //}
            //string decryptProductGroupCode = CommonServices.DecryptPassword(ProductGroupCode);
            
            DataTable dataTable = await _Goods_DAL.GetProductList(CompanyCode, ProductGroupCode);

            List<GetCompanyWiseProductListDTO> list = new List<GetCompanyWiseProductListDTO>();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                var obj = new GetCompanyWiseProductListDTO
                {
                    CompanyCode = row["CompanyCode"].ToString(),
                    CompanyName = row["CompanyName"].ToString(),
                    ProductId = CommonServices.EncryptPassword(row["ProductId"].ToString()),
                    ProductName = row["ProductName"].ToString(),
                    ProductGroupID = CommonServices.EncryptPassword(row["ProductGroupID"].ToString()),
                    ProductGroupName = row["ProductGroupName"].ToString(),
                    Specification = row["Specification"].ToString(),
                    UnitId = CommonServices.EncryptPassword(row["UnitId"].ToString()),
                    Unit = row["Unit"].ToString(),
                    Price = row["Price"] != DBNull.Value ? Convert.ToDecimal(row["Price"]) : 0,
                    DiscountAmount = row["DiscountAmount"] != DBNull.Value ? Convert.ToDecimal(row["DiscountAmount"]) : 0,
                    DiscountPct = row["DiscountPct"] != DBNull.Value ? Convert.ToDecimal(row["DiscountPct"]) : 0,
                    ImagePath = row["ImagePath"].ToString(),
                    TotalPrice = row["TotalPrice"] != DBNull.Value ? Convert.ToDecimal(row["TotalPrice"]) : 0,
                    SellerId = CommonServices.EncryptPassword(row["SellerId"].ToString()),
                    AvailableQty = Convert.ToInt32(row["AvailableQty"])
                };
                DateTime? endDate = null;
                if (row["EndDate"] != DBNull.Value)
                {
                    endDate = Convert.ToDateTime(row["EndDate"]);
                    if (endDate <= DateTime.Now)
                    {

                        obj.TotalPrice = obj.Price;
                        obj.DiscountAmount = 0;
                        obj.DiscountPct = 0;
                    }
                }
                list.Add(obj);
            }

            return list;
        }



        public async Task<List<GetRecommendedProductListDTO>> GetRecommendedProductList(string CompanyCode, string ProductId)
        {
            int decryptProductId = int.Parse(CommonServices.DecryptPassword(ProductId));
            //string decryptCompanyCode = CommonServices.DecryptPassword(CompanyCode);
            DataTable dataTable = await _Goods_DAL.GetRecommendedProductList(CompanyCode, decryptProductId);

            List<GetRecommendedProductListDTO> list = new List<GetRecommendedProductListDTO>();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                GetRecommendedProductListDTO obj = new GetRecommendedProductListDTO();
                obj.ProductId = CommonServices.EncryptPassword(row["ProductId"].ToString());
                obj.ProductName = row["ProductName"].ToString();
                obj.ImagePath = row["ImagePath"].ToString();
                obj.CompanyCode = row["CompanyCode"].ToString();
                obj.CompanyName = row["CompanyName"].ToString();
                obj.AvailableQty = Convert.ToDecimal(row["AvailableQty"]);
                obj.TotalPrice = Convert.ToDecimal(row["TotalPrice"]);

                list.Add(obj);

            }

            return list;
        }

    }
}
