using System.ComponentModel.DataAnnotations.Schema;

namespace DoubleLStore.WebApp.Entities
{
    public class Notifi
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public Users User { get; set; }
        public string Message { get; set; }
        public DateTime DateCreated { get; set; }
        public bool isNewNotifi { get; set; }
    }
}
