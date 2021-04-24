using System;

namespace SendMail
{
    class Program
    {
        static void Main(string[] args)
        {
            var mail = new MailPostMan();
            mail.SmtpServer = "*.*.*.*";
            mail.Subject = "subject";
            mail.BodyText = "body";
            mail.AddFrom("", "hoge@example.com");
            mail.AddTo("", "hoge@example.com");
            var ret = mail.SendMailAsync().Result;
        }
    }
}
