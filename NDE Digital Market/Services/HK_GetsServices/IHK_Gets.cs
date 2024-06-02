using NDE_Digital_Market.Model;
using NDE_Digital_Market.Model.DTO;

namespace NDE_Digital_Market.Services.HK_GetsServices
{
    public interface IHK_Gets
    {
        Task<List<GetPaymentMethodListDTO>> PaymentMethodGetAsync();
        Task<List<GetPaymentMethodListDTO>> BankNameGetAsync(string preferredPM);
        Task<List<GetReturnTypeListDTO>> GetReturnListAsync();
    }
}
