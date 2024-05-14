namespace NDE_Digital_Market.Model.DTO
{
    public class UserInfoUpdateDTO
    {
        public string? CompanyCode { get; set; }
        public string UserId { get; set; }
        public string? UserCode { get; set; }
        public bool? IsBuyer { get; set; }
        public bool? IsSeller { get; set; }
        public bool? IsAdmin { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public string? Password { get; set; }
        public string? OldPassword { get; set; }

        public string? Address { get; set; }
        public DateTime? TimeStamp { get; set; }
        public int? IsActive { get; set; }
    }
}
