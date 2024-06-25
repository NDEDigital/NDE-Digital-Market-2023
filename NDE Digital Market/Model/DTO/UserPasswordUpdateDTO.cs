namespace NDE_Digital_Market.Model.DTO
{
    public class UserPasswordUpdateDTO
    {
        //public string? UserCode { get; set; }
        public string UserId { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}
