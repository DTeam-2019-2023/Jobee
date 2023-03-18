using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Security;
using System;
using NuGet.Protocol.Plugins;

namespace Jobee_API.Tools
{
    public class MailSettings
    {
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }

    public interface IEmailService
    {
        Task<bool> SendEmailAsync(EmailData emailData);
    }

    public class EmailData
    {
        public string EmailToId { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
    }
    public class SendMailService : IEmailService
    {
        private readonly MailSettings mailSettings;

        private readonly ILogger<SendMailService> logger;

        // mailSetting được Inject qua dịch vụ hệ thống
        // Có inject Logger để xuất log
        public SendMailService(IOptions<MailSettings> _mailSettings, ILogger<SendMailService> _logger)
        {
            mailSettings = _mailSettings.Value;
            logger = _logger;
            logger.LogInformation("Create SendMailService");
        }
        public async Task<bool> SendEmailAsync(EmailData data)
        {
            var message = new MimeMessage();
            //message.Sender = new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail);
            message.From.Add(new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail));
            message.To.Add(MailboxAddress.Parse(data.EmailToId));
            message.Subject = data.EmailSubject;

            var builder = new BodyBuilder();
            builder.HtmlBody = data.EmailBody;
            message.Body = builder.ToMessageBody();

            // dùng SmtpClient của MailKit
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
                await smtp.SendAsync(message);
            }
            catch (Exception ex)
            {
                // Gửi mail thất bại, nội dung email sẽ lưu vào thư mục mailssave
                Directory.CreateDirectory("mailssave");
                var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
                await message.WriteToAsync(emailsavefile);

                logger.LogInformation("Lỗi gửi mail, lưu tại - " + emailsavefile);
                logger.LogError(ex.Message);
            }

            smtp.Disconnect(true);
            smtp.Dispose();

            logger.LogInformation("send mail to: " + data.EmailToId);
            return true;
        }

        private static string MAIL_CONTENT_CONFIRM
        {
            get
            {
                return @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional //EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"" xmlns:o=""urn:schemas-microsoft-com:office:office"" xmlns:v=""urn:schemas-microsoft-com:vml"" lang=""en"">
<head>
  <link rel=""stylesheet"" type=""text/css"" hs-webfonts=""true"" href=""https://fonts.googleapis.com/css?family=Lato|Lato:i,b,bi"">
  <title>Email template</title>
  <meta property=""og:title"" content=""Email template"">
  <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"">
  <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
  
</head>
<head>
<style>
@media only screen and (max-width: 479px) { display:table-cell !important; }
    .button-24:hover,
    .button-24:active {
      background-color: initial;
      background-position: 0 0;
      color: #FF4742;
    }
    .button-24:active {
      opacity: .5;
    }
  </style>
</head>
<body bgcolor=""#f5f8fa"" style=""width: 100%; margin: auto 0; padding:0; font-family:Lato, sans-serif; font-size:18px; color:#33475b; word-break:break-word"">
  <div style=""margin: auto; width: 600px; background-color: white;"">
    <table role=""presentation"" width=""100%"">
      <tr>
        <td bgcolor=""#eeeeee"" align=""center"" style=""vertical-align: top; color: white;"" valign=""top"">
          <img loading=""lazy"" alt=""logo"" src=""https://i.postimg.cc/WpFgxnyX/Logo.png"" style=""height: 100px;"" height=""100"">
        </td>
      </tr>
    </table>
    <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""10px"" style=""padding: 30px 30px 30px 60px;"">
      <tr>
        <td style=""vertical-align: top;"" valign=""top"">
          <p>Hi %USERNAME%,</p>
          <p style=""font-weight: 100;"">
            This is mail send from Dcontact.cc use to verify your mail. It will be available within 5 minutes.<br/><i>don't share this with else</i>
          </p>
        </td>
      </tr>
      <tr>
        <td align=""center"" style=""vertical-align: top;"" valign=""top"">
          <a class=""button-24"" href=""%CALLBACK_URL%"" style=""background: #FF4742; border: 1px solid #FF4742; border-radius: 6px; box-shadow: rgba(0, 0, 0, 0.1) 1px 2px 4px; box-sizing: border-box; color: #FFFFFF; cursor: pointer; display: inline-block; font-family: nunito,roboto,proxima-nova,'proxima nova',sans-serif; font-size: 16px; font-weight: 800; line-height: 16px; min-height: 40px; outline: 0; padding: 12px 14px; text-align: center; text-rendering: geometricprecision; text-transform: none; user-select: none; -webkit-user-select: none; touch-action: manipulation; vertical-align: middle; text-decoration: none;"">Verify your email</a>
        </td>
      </tr>
    </table>
  </div>
</body>
</html>";
            }
        }
        public static string GetConfirmMail(string USERNAME, string CALLBACK_URL)
        {
            var content = MAIL_CONTENT_CONFIRM;
            content = content.Replace($"%{nameof(CALLBACK_URL)}%", CALLBACK_URL);
            content = content.Replace($"%USERNAME%", USERNAME);
            return content;
        }
    }
}
