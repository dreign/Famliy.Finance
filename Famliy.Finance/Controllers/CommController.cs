using Famliy.Finance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Famliy.Finance.Controllers
{
    [Authorize]
    public class CommController : BaseController
    {
        public override void InitBreadCrumbs()
        {
            List<BreadCrumbs> navList = new List<BreadCrumbs>();
            navList.Add(new BreadCrumbs() { ID = 1, Text = "理财", Controll = "Home", Action = "Index" });
            navList.Add(new BreadCrumbs() { ID = 2, Text = "理财工具", Controll = "Comm", Action = "Index" });
            ViewData["NavList"] = navList;
        }

        // GET: Comm
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Calc()
        {
            return View();
        }

        public ActionResult Fangdai()
        {
            return View();
        }
        public ActionResult Shouyi()
        {
            return View();
        }
    }
}