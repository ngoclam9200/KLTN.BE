using System.ComponentModel.DataAnnotations.Schema;

namespace DoubleLStore.WebApp.Entities
{
    public class Carts
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public string UserId { get; set; }
        public Users User { get; set; }
        public Products Product { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
