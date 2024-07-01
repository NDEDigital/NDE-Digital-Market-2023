
using NDE_Digital_Market.Model;
using NDE_Digital_Market.Data_Access_Layer;
using System.Data;
using NDE_Digital_Market.Model.DTO;
using NDE_Digital_Market.SharedServices;

namespace NDE_Digital_Market.Services.HK_GetsServices;

public class HK_Gets : IHK_Gets
{
    private readonly HK_Gets_DAL _HK_Gets_DAL;
    public HK_Gets(HK_Gets_DAL hK_Gets_DAL)
    {
        this._HK_Gets_DAL = hK_Gets_DAL;
    }
    public async Task<List<GetPaymentMethodListDTO>> PaymentMethodGetAsync()
    {

        DataTable dataTable = await _HK_Gets_DAL.PaymentMethodGetAsync();

        List<GetPaymentMethodListDTO> list = new List<GetPaymentMethodListDTO>();
        // Check if dataTable is null
        if (dataTable == null)
        {
            return null;
        }

        foreach (DataRow row in dataTable.Rows)
        {
            GetPaymentMethodListDTO obj = new GetPaymentMethodListDTO();
            obj.PMID = CommonServices.EncryptPassword(row["PMID"].ToString());
            obj.PMName = row["PMName"].ToString();

            list.Add(obj);
        }

        return list;
    }
    public async Task<List<GetPaymentMethodListDTO>> BankNameGetAsync(string preferredPM)
    {

        int decryptpreferredPM = int.Parse(CommonServices.DecryptPassword(preferredPM));
        DataTable dataTable = await _HK_Gets_DAL.BankNameGetAsync(decryptpreferredPM);

        List<GetPaymentMethodListDTO> list = new List<GetPaymentMethodListDTO>();
        // Check if dataTable is null
        if (dataTable == null)
        {
            return null;
        }

        foreach (DataRow row in dataTable.Rows)
        {
            GetPaymentMethodListDTO obj = new GetPaymentMethodListDTO();
            obj.PMID = CommonServices.EncryptPassword(row["PMID"].ToString());
            obj.PMName = (string)row["PMName"].ToString();

            list.Add(obj);
        }

        return list;
    }

    public async Task<List<GetReturnTypeListDTO>> GetReturnListAsync()
    {

        DataTable dataTable = await _HK_Gets_DAL.GetReturnListAsync();

        List<GetReturnTypeListDTO> list = new List<GetReturnTypeListDTO>();
        // Check if dataTable is null
        if (dataTable == null)
        {
            return null;
        }

        foreach (DataRow row in dataTable.Rows)
        {
            GetReturnTypeListDTO obj = new GetReturnTypeListDTO();
            obj.ReturnTypeId = CommonServices.EncryptPassword(row["ReturnTypeId"].ToString());
            obj.ReturnTypeName = row["ReturnTypeName"].ToString();

            list.Add(obj);
        }

        return list;
    }


}
