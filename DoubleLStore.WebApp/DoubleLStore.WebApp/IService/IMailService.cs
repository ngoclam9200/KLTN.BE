using DoubleLStore.WebApp.ViewModels.Mail;

namespace DoubleLStore.WebApp.IService
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
