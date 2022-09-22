using System.ComponentModel.DataAnnotations.Schema;

namespace DoubleLStore.WebApp.Entities
{
    public class ImageProduct
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Url   { get; set; }
        public string ProductId     { get; set; }
        public Products Product { get; set; }
        public bool isDefaut { get; set; }

    }
}
