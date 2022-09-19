using System.ComponentModel.DataAnnotations.Schema;

namespace DoubleLStore.WebApp.Entities
{
    public class Roles
    {
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
       public string Name { get; set; }
       public DateTime DateCreated { get; set; }


    }
}
