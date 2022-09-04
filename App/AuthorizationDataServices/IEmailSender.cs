using System.Threading.Tasks;

public interface IEmailSender
{
    Task SendEmailAsync(string email, string v1, string v2);
}