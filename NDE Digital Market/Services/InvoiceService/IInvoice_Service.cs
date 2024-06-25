using NDE_Digital_Market.Model.DTO;

namespace NDE_Digital_Market.Services.InvoiceService
{
    public interface IInvoice_Service
    {
        Task<GetOrderInvoiceDataForBuyerByOrderMasterIdDTO> GetInvoiceDataForBuyer(string OrderMasterId);

        Task<GetOrderInvoiceDataForSellerByOrderMasterIdDTO> GetInvoiceDataForSeller(string SSMId);

    }
}
