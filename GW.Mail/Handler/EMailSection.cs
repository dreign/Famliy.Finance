using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;

namespace GW.Mail.Handler
{
    public class EMailSection : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            NameValueSectionHandler baseHandler = new NameValueSectionHandler();
            NameValueCollection configs = (NameValueCollection)baseHandler.Create(parent, configContext, section);
            return configs;
        }
    }
}
