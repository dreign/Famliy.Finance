using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Configuration;
using GW.Mail.Entity;

namespace GW.Mail
{
    public class ConfigurationInfo
    {
        //private static ConfigurationInfo _instance = null;
        private static Dictionary<string, ConfigurationInfo> _instanceDict = new Dictionary<string, ConfigurationInfo>();
        public static ConfigurationInfo GetInstance(string key)
        {
            if (_instanceDict.Keys.Contains(key))
                return _instanceDict[key];
            else
            {
                NameValueCollection nameValues = (NameValueCollection)ConfigurationSettings.GetConfig(key);
                string subject = nameValues["subject"];
                string server = nameValues["server"];
                int port = string.IsNullOrEmpty(nameValues["port"]) ? 0 : Convert.ToInt32(nameValues["port"]);                
                string sender = nameValues["sender"];
                string receivers = nameValues["receivers"];
                string cc = nameValues["cc"];
                string bcc = nameValues["bcc"];
                string userName = nameValues["userName"];
                string password = nameValues["password"];
                bool enableSSL = nameValues["enableSSL"] == "1";
                ConfigurationInfo item = new ConfigurationInfo();
                item.DeflautSubject = subject;
                item.AddressInfo = new MailAddressInfo(sender, receivers, cc, bcc);
                item.ServerInfo = new MailServerInfo(server, port, userName, password);
                _instanceDict.Add(key, item);
                return item;
            }
        }
        public static MailInfo GetMainInfo(string key)
        {
            ConfigurationInfo configurationInfo = GetInstance(key);
            MailInfo mainInfo = new MailInfo() {
                MailAddressInfo = configurationInfo.AddressInfo,
                MailServerInfo = configurationInfo.ServerInfo,
                Subject = configurationInfo.DeflautSubject,
                EnableSsl = configurationInfo.enableSSL
            };
            return mainInfo;
        }
         public bool enableSSL { get; set; }
        
        public MailAddressInfo AddressInfo { get; set; }
        public MailServerInfo ServerInfo { get; set; }
        public string DeflautSubject { get; set; }
    }
}
