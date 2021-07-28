using Ext.Direct.Mvc;
using MesJolisCotillons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MesJolisCotillons.Area._Admin
{
    [AdminAuthorize]
    public class AdminController : Controller
    {
        // GET: Admin_

        public ActionResult Index()
        {
            return View("~/Views/Admin/Admin.cshtml");
        }
    }
}