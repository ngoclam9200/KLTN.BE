using System.ComponentModel.DataAnnotations.Schema;

namespace DoubleLStore.WebApp.Entities
{
    public class Products
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double Originalprice { get; set; }
        public int Stock { get; set; }
        public int Discount { get; set; }
        public string Image { get; set; }
        public DateTime DateCreated { get; set; }
        public Categories Category { get; set; }
      
     }
}
