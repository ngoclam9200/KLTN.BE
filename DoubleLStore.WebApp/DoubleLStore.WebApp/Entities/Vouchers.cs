using System.ComponentModel.DataAnnotations.Schema;

namespace DoubleLStore.WebApp.Entities
{
    public class Vouchers
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Code { get; set; }
        public double Discountprice { get; set; }
        public double Discountfreeship { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateExpiration { get; set; }
        public bool isDeleted { get; set; }
        public int AmountInput { get; set; }
        public int AmountRemaining { get; set; }
    }
}
