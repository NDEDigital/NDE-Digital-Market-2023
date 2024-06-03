namespace NDE_Digital_Market.Model.DTO
{
    public class GetCompanyListByStatusDTO
    {
        public string? CompanyID { get; set; }
        public int? MaxUser { get; set; }
        public string? CompanyCode { get; set; }
        public string? CompanyName { get; set; }
        public string? Email { get; set; }
        public string? CompanyAdminId { get; set; }
        public string? CompanyImage { get; set; } // Assume byte[] for image data
        public DateTime? CompanyFoundationDate { get; set; } // Nullable for potentially missing values
        public string? BusinessRegistrationNumber { get; set; }
        public string? TaxIdentificationNumber { get; set; }
        public string? TradeLicense { get; set; }
        public string? PreferredPaymentMethodID { get; set; }
        public string? PreferredPaymentMethodName { get; set; }
        public string? BankNameID { get; set; }
        public string? BankName { get; set; }
        public string? AccountNumber { get; set; }
        public string? AccountHolderName { get; set; }
    }
}
