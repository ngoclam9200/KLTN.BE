using System.ComponentModel.DataAnnotations.Schema;

namespace DoubleLStore.WebApp.Entities
{
    public class AddressUsers
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Address { get; set; }
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public string WardCode { get; set; }
        public Users User { get; set; }
        public bool isDeleted   { get; set; }
        public bool isAddressDefaut { get; set; }
    
        
    }
}
