using System.ComponentModel.DataAnnotations.Schema;
namespace DoubleLStore.WebApp.Entities
{
    public class ProductDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string ProductId { get; set; }
        public Products Product { get; set; }
        public int S { get; set; }
        public int M { get; set; }
        public int L { get; set; }
        public int XL { get; set; }
        public int XXL { get; set; }

    }
}
