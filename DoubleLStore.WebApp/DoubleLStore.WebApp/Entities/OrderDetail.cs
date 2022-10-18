using System.ComponentModel.DataAnnotations.Schema;

namespace DoubleLStore.WebApp.Entities
{
    public class OrderDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string OrderId { get; set; }
        public Orders Order { get; set; }
        public string ProductId { get; set; }
        public Products Product { get; set; }
        public int ProductCount { get; set; }
    }
}
