using NDE_Digital_Market.Data_Access_Layer;
using NDE_Digital_Market.Model;
using NDE_Digital_Market.Model.DTO;
using NDE_Digital_Market.SharedServices;
using System.Data;

namespace NDE_Digital_Market.Services.UnitService
{
    public class Unit_Service : IUnit_Service
    {
        private readonly Unit_DAL _unit_DAL;
        public Unit_Service(Unit_DAL unit_DAL)
        {
            _unit_DAL = unit_DAL;
        }
        public async Task<List<GetUnitListDTO>> GetUnitListAsync(bool? isActive)
        {
            DataTable dataTable = await _unit_DAL.GetUnitListAsync(isActive);

            List<GetUnitListDTO> unitlist = new List<GetUnitListDTO>();
            // Check if dataTable is null
            if (dataTable == null)
            {
                return null;
            }
            foreach (DataRow row in dataTable.Rows)
            {
                GetUnitListDTO modelObj = new GetUnitListDTO
                {
                    UnitId = CommonServices.EncryptPassword((row["UnitId"].ToString())),
                    Name = row["Name"].ToString(),
                    Description = row["Description"].ToString(),
                    IsActive = Convert.ToBoolean(row["IsActive"]),
                    IsConversion = Convert.ToBoolean(row["IsConversion"])
                };

                unitlist.Add(modelObj);
            }
            return unitlist;
        }

        public async Task<object> PostUnit(UnitCreationDTO unit)
        {
            UnitModel model = new UnitModel();
            model.Name = unit.Name;
            model.Description = unit.Description;
            model.AddedBy = unit.AddedBy;
            model.AddedPC = unit.AddedPC;
            return await _unit_DAL.PostUnit(model);
        }

        public async Task<object> PutUnit(UpdateUnitDTO unit)
        {
            int decryptedId = int.Parse(CommonServices.DecryptPassword(unit.UnitId));

            UnitModel model = new UnitModel();
            model.UnitId = decryptedId;

            model.Name = unit.Name;
            model.Description = unit.Description;
            model.UpdatedBy = unit.UpdatedBy;
            model.UpdatedPC = unit.UpdatedPC;
            model.IsConversion = unit.IsConversion;
            return await _unit_DAL.PutUnit(model);
        }

        public async Task<object> UpdateUnitByUnitID(string unitID, bool isActive)
        {
            try
            {
                string decryptedUserId = CommonServices.DecryptPassword(unitID);
                int decryptedUserIdInt;
                int.TryParse(decryptedUserId, out decryptedUserIdInt);
                return await _unit_DAL.UpdateUnitByUnitID(decryptedUserIdInt, isActive);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (new { message = "Can't decrypt the Id." });
            }


        }

        public async Task<object> UpdateUnitsByUnitID(string unitIDs, bool isActive)
        {
            string[] ids = unitIDs.Split(',');

            // Assuming each decrypted ID should be an int, and you want an array of decrypted int IDs.
            int[] decryptedIds = new int[ids.Length];

            for (int i = 0; i < ids.Length; i++)
            {
                string decryptedIdString = CommonServices.DecryptPassword(ids[i]);
                decryptedIds[i] = int.Parse(decryptedIdString); // Convert the decrypted string to an int
            }
            return await _unit_DAL.UpdateUnitsByUnitID(decryptedIds, isActive);


        }
    }
}
