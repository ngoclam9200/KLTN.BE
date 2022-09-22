using System.ComponentModel.DataAnnotations.Schema;

namespace DoubleLStore.WebApp.Entities
{
    public class CostProduct
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        public double TotalCost { get; set; }
        public string Month { get; set; }

        public string ProductId { get; set; }
        public Products Product { get; set; }
    }
}
