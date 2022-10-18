using System.ComponentModel.DataAnnotations.Schema;

namespace DoubleLStore.WebApp.Entities
{
    public class ChatUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string ChatId { get; set; }
        public string Message { get; set; }
        public bool isNewMessageAdmin { get; set; }
        public bool isNewMessageUser { get; set; }
        public string UserId { get; set; }
        public Users User { get; set; }
        public int DisplayPriority { get; set; }

    }
}
