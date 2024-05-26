using NDE_Digital_Market.Model;
using NDE_Digital_Market.Model.DTO;

namespace NDE_Digital_Market.Services.ProductGroupService
{
    public interface IProductGroup_Service
    {
        Task<object> CreateProductGroupsAsync(InsertProductGroupDTO productGroupsDto);


        Task<object> UpdateProductGroupsAsync(UpdateProductGroupDTO productGroupsDto);


        Task<List<GetAllProductGroupDTO>> GetProductGroupsListAsync();


        Task<List<GetProductGroupListByStatusDTO>> GetProductGroupsListByStatus(Int32? status = null);


        Task<object> MakeGroupActiveOrInactiveAsync(string groupIds, bool? IsActive);
    }
}
