using MesJolisCotillons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;


#region CurrentUser
public class CurrentUser
{
    public static User User
    {
        get
        {
            User_Repository user_Repository = new User_Repository();
            User result = null;
            int user_ID;

            if (Thread.CurrentPrincipal.Identity.IsAuthenticated && Int32.TryParse(Thread.CurrentPrincipal.Identity.Name, out user_ID))
            {
                result = user_Repository.FindAllUser().Where(item => item.User_ID == user_ID).FirstOrDefault();
            }


            return result;
        }
    }
}
#endregion


