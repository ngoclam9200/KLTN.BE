namespace DoubleLStore.WebApp.ViewModels.User
{
    public class RegisterUserRequest
    {
        string RoleId { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phonenumber { get; set; }
        public string Gender { get; set; }
        public string Avatar { get; set; }

    }
}
