using System.ComponentModel.DataAnnotations.Schema;

namespace DoubleLStore.WebApp.Entities
{
    public class Users

    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string RoleId { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phonenumber { get; set; }
        public DateTime DateCreated { get; set; }
        public Roles Role { get; set; }
        public string Avatar {get; set;}
        public string Gender { get; set;}
        public bool isDeleted { get; set; }

    }
}
