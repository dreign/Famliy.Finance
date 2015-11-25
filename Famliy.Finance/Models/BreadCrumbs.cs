using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Famliy.Finance.Models
{

    /// <summary>
    /// 面包屑导航
    /// </summary>
    public class BreadCrumbs
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public string Controll { get; set; }
        public string Action { get; set; }
        public string Url { get; set; }       
    }
    /// <summary>
    /// 站点地图扩展
    /// </summary>
    public static class MvcSiteMapExtensions
    {
        public static MvcHtmlString GeneratorNav(this HtmlHelper html, string url)
        {
            return MvcHtmlString.Create("");
        }
    }
}
