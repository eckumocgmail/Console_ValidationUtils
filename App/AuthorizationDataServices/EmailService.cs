
using ApplicationCommon.CommonTypes;

using MailKit.Net.Pop3;
using MailKit.Security;
using MimeKit;

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Сервис умеет отправлять электронную почту и считывать входящие письма
/// </summary>
public class EmailService: IEmailSender
{
    private string emailName;
    private string emailAddress;
    private string emailPassword;
    private string smtpHost;
    private int smtpPort;        
    private string popHost;
    private int popPort;
       

    public EmailService()
    {
        this.emailName = "Администрация сайта";
        this.emailAddress = "kba-2019@mail.ru";
        this.emailPassword = "eckumoc1423";
        this.smtpHost = "smtp.mail.ru";
        this.smtpPort = 587;           
        this.popHost = "pop.mail.ru";
        this.popPort = 995;
    }


    /// <summary>
    /// Отправка сообщения по электронной почте
    /// </summary> 
    public void SendEmail(string email, string subject, string message)
    {
        /*using (var smtp = new MailKit.Net.Smtp.SmtpClient())
        {
            smtp.ServerCertificateValidationCallback = (s, c, h, e) =>
            {
                System.Console.Write(s.GetType().FullName);
                System.Console.WriteLine(s);

                System.Console.Write(s.GetType().FullName);
                System.Console.WriteLine(c);

                System.Console.Write(s.GetType().FullName);
                System.Console.WriteLine(h);

                System.Console.Write(s.GetType().FullName);
                System.Console.WriteLine(e);
                return true;
            };
            
            smtp.Connect(smtpHost, smtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailAddress, emailPassword);
        */
        /*var emailMessage = new MimeMessage();

        emailMessage.From.Add(new MailboxAddress(this.emailName, emailAddress));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = message
        };

        var data = ToByteArray(emailMessage);
        string text = Encoding.UTF8.GetString(data);

        Console.WriteLine(text);*/
        /*
                
            smtp.Send(emailMessage);
            smtp.Disconnect(true);
        }*/
    }

    private byte[] ToByteArray(MimeMessage emailMessage)
    {
        using (var stream = new MemoryStream())
        {
            emailMessage.WriteTo(GetFormatOptions(), stream);
            byte[] data = stream.ToArray();
            return data;
        }
            
    }

    private FormatOptions GetFormatOptions()
    {
        throw new NotImplementedException();
    }


    /// <summary>
    /// Отправка сообщения по электронной почте с прикреплёнными файлами
    /// </summary> 
    public void SendEmail(string email, string subject, string message, 
                            TypeFile[] resources)
    {
        using (var smtp = new MailKit.Net.Smtp.SmtpClient())
        {
            smtp.ServerCertificateValidationCallback = (s, c, h, e) =>
            {
                return true;
            };

            smtp.Connect(smtpHost, smtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailAddress, emailPassword);
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(this.emailName, emailAddress));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            var builder = new BodyBuilder();
                
            builder.TextBody = message;
            if (resources != null)
            {
                foreach(TypeFile resource in resources)
                {
                    System.IO.File.WriteAllBytes(resource.Name, resource.Data);
                    builder.Attachments.Add(resource.Name);
                }
            }                             
            emailMessage.Body = builder.ToMessageBody();
               
            smtp.Send(emailMessage);
            smtp.Disconnect(true);
        }
    }


    /// <summary>
    /// Получение входящих сообщений
    /// </summary>
    public void Recieve()
    {
        using (var client = new Pop3Client())
        {                
            client.ServerCertificateValidationCallback = (s, c, h, e) => true;
            client.Connect(popHost, popPort);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate(emailAddress, this.emailAddress );
            for (int i = 0; i < client.Count; i++)
            {
                MimeMessage message = client.GetMessage(i);                    
            }
            client.Disconnect(true);
        }
    }

    public Task SendEmailAsync(string email, string v1, string v2)
    {
        throw new NotImplementedException();
    }
}
