using NDE_Digital_Market.Data_Access_Layer;
using NDE_Digital_Market.Model.DTO;
using NDE_Digital_Market.SharedServices;
using System.Data;

namespace NDE_Digital_Market.Services.GetBuyerInAdminService
{
    public class GetBuyerInAdmin_Service : IGetBuyerInAdmin_Service
    {
        private readonly GetBuyerInAdmin_DAL _getBuyerInAdmin_DAL;

        public GetBuyerInAdmin_Service(GetBuyerInAdmin_DAL getBuyerInAdmin_DAL)
        {
            _getBuyerInAdmin_DAL = getBuyerInAdmin_DAL;
        }
        public async Task<List<GetCompanySellerListDTO>> CompanySellerDetails(bool IsBuyer, bool IsActive)
        {
            
            DataTable dataTable = await _getBuyerInAdmin_DAL.CompanySellerDetails(IsBuyer, IsActive);

            List<GetCompanySellerListDTO> list = new List<GetCompanySellerListDTO>();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                GetCompanySellerListDTO obj = new GetCompanySellerListDTO();
                obj.UserId = CommonServices.EncryptPassword(row["UserId"].ToString());
                //UserId = reader.GetInt32(userId),
                obj.FullName = row["FullName"].ToString();
                obj.PhoneNumber = row["PhoneNumber"].ToString();
                obj.Email = row["Email"].ToString();
                obj.Address = row["Address"].ToString();
                obj.AddedDate = (DateTime)(row["AddedDate"] as DateTime?);
                obj.IsActive = row["IsActive"] as bool? ?? IsActive;
                obj.IsBuyer = row["IsBuyer"] as bool? ?? IsBuyer;
            }

            return list;
        }
    }
}
