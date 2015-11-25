using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
namespace GW.Mail.Entity
{
    public class MailInfo
    {
        public static readonly char[] MailAddrSpliters = new char[] { ',', ';', ' ' };
        public static readonly string ResImgNameRegex = @"[\w-_]+.\w+$";
        public static readonly string ResImgPathMatchRegex = "(?<=<\\s*img\\s*.*?src\\s*=\\s*\").*?\\.\\w+(?=\")";

        public MailInfo()
        {
        }

        public MailInfo(string subject) : this(subject, string.Empty, false)
        {
        }

        public MailInfo(string subject, string body, bool isHtml)
        {
            this.Priority = MailPriority.Normal;
            this.Subject = subject;
            this.IsHtml = this.IsHtml;
            this.Encoding = System.Text.Encoding.UTF8;
            this.ImgDirectory = string.Empty;
        }

        public MailInfo(string from, string to, string cc, string subject, string body, params string[] attachAddress)
        {
            this.MailAddressInfo = new GW.Mail.Entity.MailAddressInfo(from, to, cc);
            this.Subject = subject;
            this.Body = body;
            this.AttachmentList.AddRange(attachAddress);
        }

        public MailInfo(string from, string to, string cc, string bcc, string subject, string body, params string[] attachAddress)
        {
            this.MailAddressInfo = new GW.Mail.Entity.MailAddressInfo(from, to, cc, bcc);
            this.Subject = subject;
            this.Body = body;
            this.AttachmentList.AddRange(attachAddress);
        }

        public List<string> AttachmentList { get; set; }

        public string Body { get; set; }

        public bool EnableSsl { get; set; }

        public System.Text.Encoding Encoding { get; set; }

        public string ImgDirectory { get; set; }

        public bool IsHtml { get; set; }

        public bool LinkImgRes { get; set; }

        public GW.Mail.Entity.MailAddressInfo MailAddressInfo { get; set; }

        public GW.Mail.Entity.MailServerInfo MailServerInfo { get; set; }

        public MailPriority Priority { get; set; }

        public string Subject { get; set; }
    }
}

