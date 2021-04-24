using System;
using System.Threading.Tasks;
using MimeKit;

namespace SendMail
{
    class MailPostMan
    {
        private MimeKit.MimeMessage MimeMessage = null;
        private MimeKit.TextPart TextPart = null;
        public string SmtpServer;
        public string Subject
        {
            get { return this.MimeMessage.Subject; }
            set { this.MimeMessage.Subject = value; }
        }
        public string BodyText
        {
            get { return this.TextPart.Text; }
            set { this.TextPart.Text = value; }
        }
        public MailPostMan()
        {
            this.MimeMessage = new MimeMessage();
            this.TextPart = new TextPart();
        }
        public void AddFrom(string name, string addr)
        {
            this.MimeMessage.From.Add(new MailboxAddress(name, addr));
        }
        public void ClearFrom()
        {
            this.MimeMessage.From.Clear();
        }
        public void AddTo(string name, string addr)
        {
            this.MimeMessage.To.Add(new MailboxAddress(name, addr));
        }
        public void ClearTo()
        {
            this.MimeMessage.To.Clear();
        }
        public async Task<bool> SendMailAsync()
        {
            return await this.SendMailAsync("","");
        }
        public async Task<bool> SendMailAsync(string authUser, string authPass)
        {
            if (this.MimeMessage.From.Count == 0)
                throw new Exception("送信元アドレスが設定されていません");
            if (this.MimeMessage.To.Count == 0)
                throw new Exception("送信先アドレスが設定されていません");
            if (this.MimeMessage.Subject is null 
                || this.MimeMessage.Subject.CompareTo("") == 0)
                throw new Exception("件名が設定されていません");
            if (this.TextPart is null
                || this.TextPart.Text.CompareTo("") == 0)
                throw new Exception("本文が設定されていません");

            this.MimeMessage.Body = this.TextPart;

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync(this.SmtpServer);
                if (authUser.CompareTo("") != 0)
                    await client.AuthenticateAsync(authUser, authPass);
                await client.SendAsync(this.MimeMessage);
                await client.DisconnectAsync(true);
            }
            return true;
        }
    }
}
 