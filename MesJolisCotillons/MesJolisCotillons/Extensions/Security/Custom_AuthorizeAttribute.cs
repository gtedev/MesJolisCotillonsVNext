using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


#region AdminAuthorizeAttribute
public class AdminAuthorizeAttribute : AuthorizeAttribute
{
    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
        var isAuthorized = base.AuthorizeCore(httpContext);
        if (!isAuthorized)
        {
            return false;
        }

        if (CurrentUser.User.Admin_User != null)
        {
            return true;
        }
        else
        {
            FormsAuthentication.SignOut();
            return false;
        }
    }
}
#endregion

#region CustomerAuthorizeAttribute
public class CustomerAuthorizeAttribute : AuthorizeAttribute
{
    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
        var isAuthorized = base.AuthorizeCore(httpContext);
        if (!isAuthorized)
        {
            return false;
        }

        if (CurrentUser.User.Customer_User != null)
        {
            return true;
        }
        else
        {
            FormsAuthentication.SignOut();
            return false;
        }
    }
}
#endregion

#region AjaxOrChildActionOnlyAttribute
public class AjaxOrChildActionOnlyAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (!filterContext.HttpContext.Request.IsAjaxRequest() &&
            !filterContext.IsChildAction
        )
        {
            filterContext.Result = new HttpNotFoundResult();
        }
    }
}
#endregion
