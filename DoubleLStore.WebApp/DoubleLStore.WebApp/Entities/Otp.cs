using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoubleLStore.WebApp.Entities
{
    public class Otp
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateExpiration { get; set; }
       
    }
}
