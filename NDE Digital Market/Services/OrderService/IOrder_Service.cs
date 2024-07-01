using NDE_Digital_Market.Model.DTO;
using static NDE_Digital_Market.Data_Access_Layer.Order_DAL;

namespace NDE_Digital_Market.Services.OrderService
{
    public interface IOrder_Service
    {
        Task<object> InsertOrderDateAsync(InsertOrderMasterDTO data);



        Task<List<GetOrderMasterDataListByStatusDTO>> GetOrderMasterData(string? status);



        Task<List<GetOrderDetailsDataListByStatusDTO>> GetOrderDetailData(string? OrderMasterId, string? status);


        Task<object> UpdateOrderStatusAsync(string orderMasterId, string? detailsCancelledId, string status);

        Task<object> SellerOrderDetailsStatusChangedAsync(updateOrderClass updateOrder);

        Task<GetSingleUserInfoDTO> getUserInfo(string UserId);



        Task<List<GetSellerOrderDataByUserIdAndStatusDTO>> GetSellerOrderBasedOnUserCodeAsync(string userid, string? status);



        Task<List<GetBuyerOrderDataByUserIdAndStatusDTO>> GetBuyerOrderBasedOnUserIDAsync(string userid, string? status);




        Task<List<GetOrderMasterDataForBuyerByUserIdDTO>> getAllOrderForBuyerAsync(string userid, string? status);




        Task<GetOrderFullDetailsForBuyerMasterDTO> getOrderDetailsForBuyerBasedOnOrderNoAsync(string OrderNo);



        Task<List<GetOrderMasterDataForSellerByCompanyCodeDTO>> getAllOrderForSellerAsync(string CompanyCode, string? status);

    }
}
