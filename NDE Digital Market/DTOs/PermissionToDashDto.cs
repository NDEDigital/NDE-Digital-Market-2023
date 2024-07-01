namespace NDE_Digital_Market.DTOs
{
    public class PermissionToDashDto
    {

        public int MenuId { get; set; }

        public int UserId { get; set; } = 0;
        public int PermissionId {  get; set; }
        public string MenuName { get; set; }
        public string FullName { get; set; }
        public string?   NavigateUrl {  get; set; }

    }
}
