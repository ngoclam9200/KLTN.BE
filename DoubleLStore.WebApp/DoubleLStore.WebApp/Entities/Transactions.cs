using System.ComponentModel.DataAnnotations.Schema;

namespace DoubleLStore.WebApp.Entities
{
    public class Transactions
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public bool isDeleted { get; set; }

    }
}
