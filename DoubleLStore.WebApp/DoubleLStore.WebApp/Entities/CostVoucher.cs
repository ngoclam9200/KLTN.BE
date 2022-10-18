using System.ComponentModel.DataAnnotations.Schema;

namespace DoubleLStore.WebApp.Entities
{
    public class CostVoucher
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public int  Cost { get; set; }
        public string VoucherId { get; set; }
        public Vouchers Voucher { get; set; }
        public DateTime DateCreated { get; set; }
        
    }
}
