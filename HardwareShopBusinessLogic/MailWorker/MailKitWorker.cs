using HardwareShopContracts.BindingModels;
using HardwareShopContracts.BusinessLogicsContracts;
using MailKit.Net.Pop3;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace HardwareShopBusinessLogic.MailWorker
{
    public class MailKitWorker : AbstractMailWorker
    {
        public MailKitWorker(ILogger<MailKitWorker> logger) : base(logger) { }

        protected override async Task SendMailAsync(MailSendInfoBindingModel info)
        {
            using var objMailMessage = new MailMessage();
            using var objSmtpClient = new SmtpClient(_smtpClientHost, _smtpClientPort);
            try
            {
                objMailMessage.From = new MailAddress(_mailLogin);
                objMailMessage.To.Add(new MailAddress(info.MailAddress));
                objMailMessage.Subject = info.Subject;
                objMailMessage.Body = info.Text;
                objMailMessage.SubjectEncoding = Encoding.UTF8;
                objMailMessage.BodyEncoding = Encoding.UTF8;
                MemoryStream ms = new(info.File);
                objMailMessage.Attachments.Add(new Attachment(ms, "report.pdf", "application/pdf"));

                objSmtpClient.UseDefaultCredentials = false;
                objSmtpClient.EnableSsl = true;
                objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                objSmtpClient.Credentials = new NetworkCredential(_mailLogin, _mailPassword);

                await Task.Run(() => objSmtpClient.Send(objMailMessage));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}