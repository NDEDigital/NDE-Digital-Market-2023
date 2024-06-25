using NDE_Digital_Market.Model.DTO;

namespace NDE_Digital_Market.Services.UnitService
{
    public interface IUnit_Service
    {

        Task<List<GetUnitListDTO>> GetUnitListAsync(bool? isActive);

        Task<object> PostUnit(UnitCreationDTO unit);

        Task<object> PutUnit(UpdateUnitDTO unit);

        Task<object> UpdateUnitByUnitID(string unitID, bool isActive);

        Task<object> UpdateUnitsByUnitID(string unitIDs, bool isActive);

    }
}
