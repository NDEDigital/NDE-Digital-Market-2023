using NDE_Digital_Market.Data_Access_Layer;
using NDE_Digital_Market.Model.DTO;
using NDE_Digital_Market.SharedServices;
using System.Data;

namespace NDE_Digital_Market.Services.CompanyAdminService
{
    public class CompanyAdmin_Service : ICompanyAdmin_Service
    {

        private readonly CompanyAdmin_DAL _companyAdmin_DAL;
        public CompanyAdmin_Service(CompanyAdmin_DAL companyAdmin_DAL)
        {
            _companyAdmin_DAL = companyAdmin_DAL;
        }
        public async Task<List<GetCompanySellerListDTO>> CompanySellerDetails(string userId, bool IsActive)
        {


            int decryptuserid = int.Parse(CommonServices.DecryptPassword(userId));
            DataTable dataTable = await _companyAdmin_DAL.CompanySellerDetails(decryptuserid,IsActive);

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
                //UserId = reader.GetInt32(userId);
                obj.FullName = row["FullName"].ToString();
                obj.PhoneNumber = row["PhoneNumber"].ToString();
                obj.Email = row["Email"].ToString();
                obj.Address = row["Address"].ToString();
                obj.AddedDate = (DateTime)(row["AddedDate"] as DateTime?);
                obj.IsActive = row["IsActive"] as bool? ?? IsActive;
                obj.CompanyCode = row["CompanyCode"].ToString();
                obj.CompanyName = row["CompanyName"].ToString();
                obj.CompanyAdminId = row["CompanyAdminId"].ToString();


                list.Add(obj);
            }

            return list;
        }



        public async Task<object> UpdateUserStatus(string userId, bool IsActive)
        {
            int decryptuserid = int.Parse(CommonServices.DecryptPassword(userId));
            return await _companyAdmin_DAL.CompanySellerDetails(decryptuserid, IsActive);
        }

    }
}
