using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Net.Mime;
using GW.Mail.Entity;
using System.Net;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using GW.Utils;

namespace GW.Mail
{
    public class MailManager
    {
        
        private static void FillAddress(MailAddressCollection addressCollection, string addressContent)
        {
            if (!string.IsNullOrEmpty(addressContent))
            {
                foreach (string str in addressContent.Split(MailInfo.MailAddrSpliters))
                {
                    if (!string.IsNullOrEmpty(str))
                    {
                        addressCollection.Add(str);
                    }
                }
            }
        }

        private static void FillMessage(MailMessage message, MailInfo mailInfo)
        {
            if (!string.IsNullOrEmpty(mailInfo.Subject))
            {
                message.Subject = mailInfo.Subject;
            }
            message.IsBodyHtml = mailInfo.IsHtml;
            message.Body = mailInfo.Body;
            MailAddressInfo mailAddressInfo = mailInfo.MailAddressInfo;
            if (mailAddressInfo != null)
            {
                if (!string.IsNullOrEmpty(mailAddressInfo.From))
                {
                    message.From = new MailAddress(mailAddressInfo.From);
                }
                FillAddress(message.To, mailAddressInfo.To);
                FillAddress(message.CC, mailAddressInfo.CC);
                FillAddress(message.Bcc, mailAddressInfo.Bcc);
            }
            if ((mailInfo.AttachmentList != null) && (mailInfo.AttachmentList.Count > 0))
            {
                foreach (string str in mailInfo.AttachmentList)
                {
                    message.Attachments.Add(new Attachment(str));
                }
            }
            if (mailInfo.IsHtml && mailInfo.LinkImgRes)
            {
                string body = mailInfo.Body;
                List<LinkedResource> imgResources = GetImgResources(ref body, mailInfo.ImgDirectory);
                AlternateView item = AlternateView.CreateAlternateViewFromString(body, mailInfo.Encoding, "text/html");
                foreach (LinkedResource resource in imgResources)
                {
                    item.LinkedResources.Add(resource);
                }
                message.AlternateViews.Add(item);
            }
        }

        private static List<LinkedResource> GetImgResources(ref string html, string imgDirectory)
        {
            List<LinkedResource> list = new List<LinkedResource>();
            MatchCollection matchs = Regex.Matches(html, MailInfo.ResImgPathMatchRegex, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            StringBuilder builder = new StringBuilder(html);
            int num = 0;
            foreach (Match match in matchs)
            {
                string input = match.Value;
                string str2 = Regex.Match(input, MailInfo.ResImgNameRegex).Value;
                string str3 = "Img" + num.ToString();
                builder.Replace(input, "cid:" + str3);
                LinkedResource item = new LinkedResource(imgDirectory + str2, "image/jpeg")
                {
                    ContentId = str3,
                    TransferEncoding = TransferEncoding.Base64
                };
                list.Add(item);
                num++;
            }
            html = builder.ToString();
            return list;
        }
        
        public static void Send(MailInfo mailInfo, bool isAsync)
        {
            try
            {
                using (MailMessage message = new MailMessage())
                {
                    MailServerInfo mailServerInfo = mailInfo.MailServerInfo;
                    if ((mailServerInfo == null) || string.IsNullOrEmpty(mailServerInfo.Server))
                    {
                        LogHelper.Write("[GW.Mail]:Web.config中配置邮件服务器尚未配置，请配置邮件服务器信息，否则无法发生邮件！");
                    }
                    SmtpClient client = new SmtpClient();//(mailServerInfo.Server, mailServerInfo.Port);
                    client.Host = mailServerInfo.Server;
                    if (mailServerInfo.Port!=0)
                        client.Port = mailServerInfo.Port;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    if (!string.IsNullOrEmpty(mailServerInfo.UserName) && !string.IsNullOrEmpty(mailServerInfo.Password))
                    {
                        client.Credentials = new NetworkCredential(mailServerInfo.UserName, mailServerInfo.Password);
                    }
                    else
                    {
                        client.UseDefaultCredentials = true;
                    }
                    client.EnableSsl = mailInfo.EnableSsl;
                    FillMessage(message, mailInfo);

                    //client.ClientCertificates = X509CertificateCollection.X509CertificateEnumerator;
                    client.SendCompleted += new SendCompletedEventHandler(MailManager.SendCompletedCallback);
                    string userToken = "test message1";
                    if (isAsync)
                    {
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.SendAsync(message, userToken);
                    }
                    else
                    {
                        message.IsBodyHtml = true;
                        client.Send(message);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write("[GW.Mail]:发送邮件失败！", ex);
            }
        }
        /// <summary>
        /// 默认使用EMailSection配置节
        /// </summary>
        public static void SendOnDefaultSection()
        {
            Send("EMailSection", string.Empty, string.Empty);
        }
        /// <summary>
        /// 默认使用EMailSection配置节
        /// </summary>
        /// <param name="body"></param>
        public static void SendOnDefaultSection(string body)
        {
            Send("EMailSection", string.Empty, body);
        }
        /// <summary>
        /// 默认使用EMailSection配置节
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public static void SendOnDefaultSection(string subject, string body)
        {
            Send("EMailSection", subject, body);
        }
        public static void Send(string mailSectionName)
        {
            Send(mailSectionName, string.Empty, string.Empty);
        }
        public static void Send(string mailSectionName, string body)
        {
            Send(mailSectionName, string.Empty, body);
        }

        public static void Send(string mailSectionName, string subject, string body)
        {
            MailInfo mailInfo = ConfigurationInfo.GetMainInfo(mailSectionName);
            if (!string.IsNullOrEmpty(subject))
            {
                mailInfo.Subject = subject;
            }
            mailInfo.Body = body;
            Send(mailInfo, false);
        }

        public static void SendTo(string mailTo, string subject, string body, string mailSectionName = "EMailSection")
        {
            if (string.IsNullOrEmpty(mailSectionName))
            {
                mailSectionName = "EMailSection";
            }
            MailInfo mailInfo = ConfigurationInfo.GetMainInfo(mailSectionName);
            if (!string.IsNullOrEmpty(subject))
            {
                mailInfo.Subject = subject;
            }
            mailInfo.Body = body;
            Send(mailInfo, false);
        }

        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            string userState = (string)e.UserState;
            if (e.Cancelled)
            {
                LogHelper.Write(string.Format("[GW.Mail]:[{0}] Send canceled.", userState));
            }
            if (e.Error != null)
            {
                LogHelper.Write(string.Format("[GW.Mail]:[{0}] {1}", userState, e.Error.ToString()));
            }
            else
            {
                LogHelper.Write("[GW.Mail]:Message sent.");
            }
        }
    }
}
