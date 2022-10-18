using System.ComponentModel.DataAnnotations.Schema;

namespace DoubleLStore.WebApp.Entities
{
    public class InfoShop
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string Address { get; set; }
        public int ProvinceID { get; set; }
        public int DistrictID { get; set; }
        public string WardCode { get; set; }
        public string Email { get; set; }
     
        public string Phonenumber { get; set; }
    }
}
