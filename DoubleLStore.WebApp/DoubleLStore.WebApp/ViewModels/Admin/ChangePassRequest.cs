namespace DoubleLStore.WebApp.ViewModels.Admin
{
    public class ChangePassRequest
    {
        public string Id { get; set; }
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }

    }
}
