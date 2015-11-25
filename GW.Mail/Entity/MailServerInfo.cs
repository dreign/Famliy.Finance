using System;
using System.Runtime.CompilerServices;

namespace GW.Mail.Entity
{
    [Serializable]
    public class MailServerInfo
    {
        public MailServerInfo()
        {
        }

        public MailServerInfo(string server, int port)
        {
            this.Server = server;
            this.Port = port;
        }

        public MailServerInfo(string server, int port, string username, string password) : this(server, port)
        {
            this.UserName = username;
            this.Password = password;
        }

        public string Password { get; set; }

        public int Port { get; set; }

        public string Server { get; set; }

        public string UserName { get; set; }
    }
}

