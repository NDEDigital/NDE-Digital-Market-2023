using SQLite;


namespace NDE_Digital_Market.Model
{
    public class WishListModel : CommonFieldsModel
    {
        [PrimaryKey]
        public int? WishListId { get; set; }
        public string? CompanyCode { get; set; }
        public int? ProductId { get; set; }
        public int? UserId { get; set; }

    }
}
