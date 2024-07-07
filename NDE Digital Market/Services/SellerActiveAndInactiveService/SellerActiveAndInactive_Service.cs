using NDE_Digital_Market.Data_Access_Layer;

using NDE_Digital_Market.Model.DTO;
using NDE_Digital_Market.SharedServices;
using System.Data;

namespace NDE_Digital_Market.Services.SellerActiveAndInactiveService
{
    public class SellerActiveAndInactive_Service : ISellerActiveAndInactive_Service
    {
        private readonly SellerActiveAndInactive_DAL _sellerActiveAndInactive_DAL;

        public SellerActiveAndInactive_Service(SellerActiveAndInactive_DAL sellerActiveAndInactive_DAL)
        {
            _sellerActiveAndInactive_DAL = sellerActiveAndInactive_DAL;
        }

        public async Task<List<GetCompanySellerDetailsDTO>> CompanySellerDetails(string? CompanyCode, bool IsSeller, bool IsActive)
        {
            try
            {
                //string decryptedcode = string.Empty;
                //if (CompanyCode != null)
                //{
                //     decryptedcode= CommonServices.DecryptPassword(CompanyCode);
                //}

                DataTable dataTable = await _sellerActiveAndInactive_DAL.CompanySellerDetails(CompanyCode, IsSeller, IsActive);

                List<GetCompanySellerDetailsDTO> list = new List<GetCompanySellerDetailsDTO>();
                // Check if dataTable is null
                if (dataTable == null)
                {
                    return null;
                }
                foreach (DataRow row in dataTable.Rows)
                {
                    GetCompanySellerDetailsDTO modelObj = new GetCompanySellerDetailsDTO();

                    modelObj.UserId = CommonServices.EncryptPassword(row["UserId"].ToString());
                    //UserId = reader.GetInt32(userId),
                    modelObj.FullName = row["FullName"].ToString();
                    modelObj.PhoneNumber = row["PhoneNumber"].ToString();
                    modelObj.Email = row["Email"].ToString();
                    modelObj.Address = row["Address"].ToString();
                    modelObj.AddedDate = (DateTime)(row["AddedDate"] as DateTime?);
                    modelObj.IsActive = row["IsActive"] as bool? ?? IsActive;
                    modelObj.IsSeller = row["IsSeller"] as bool? ?? IsActive;

                    modelObj.CompanyCode = row["CompanyCode"].ToString();
                    modelObj.CompanyAdminId = CommonServices.EncryptPassword(row["CompanyAdminId"].ToString());

                    modelObj.CompanyName = row["CompanyName"].ToString();

                    list.Add(modelObj);
                }


                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<object> UpdateSellerProductStatusAsync(string userIds, bool isActive)
        {
            string[] ids = userIds.Split(',');

            // Assuming each decrypted ID should be an int, and you want an array of decrypted int IDs.
            int[] decryptedIds = new int[ids.Length];

            for (int i = 0; i < ids.Length; i++)
            {
                string decryptedIdString = CommonServices.DecryptPassword(ids[i]);
                decryptedIds[i] = int.Parse(decryptedIdString); // Convert the decrypted string to an int
            }
            // Convert the array of decrypted integers back to a string of comma-separated values.
            string decryptedIdsString = string.Join(",", decryptedIds);

            return await _sellerActiveAndInactive_DAL.UpdateSellerProductStatusAsync(decryptedIdsString, isActive);
        }

    }
}
