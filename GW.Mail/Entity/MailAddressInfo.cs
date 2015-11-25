using System;
using System.Runtime.CompilerServices;

namespace GW.Mail.Entity
{
    public class MailAddressInfo
    {
        public MailAddressInfo(string from, string to)
        {
            this.From = from;
            this.To = to;
        }

        public MailAddressInfo(string from, string to, string cc) : this(from, to)
        {
            this.CC = cc;
        }

        public MailAddressInfo(string from, string to, string cc, string bcc) : this(from, to, cc)
        {
            this.Bcc = bcc;
        }

        public string Bcc { get; set; }

        public string CC { get; set; }

        public string From { get; set; }

        public string To { get; set; }
    }
}

