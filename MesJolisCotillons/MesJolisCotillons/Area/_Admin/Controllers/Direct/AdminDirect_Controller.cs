using Ext.Direct.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MesJolisCotillons.Area._Admin
{
    [AdminAuthorize]
    [DirectHandleError]
    public class AdminDirectController : DirectController
    {
        // GET: Admin_
        [AdminAuthorize]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return Json(new
            {
                success = true
            });
        }
    }
}