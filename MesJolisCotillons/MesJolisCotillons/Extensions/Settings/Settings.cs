using System;
using System.Collections.Generic;
using System.Web.Configuration;

public static class Settings
{
    public static string MesJolisCotillonsGmailUserName { get; set; }

    public static string MesJolisCotillonsGmailPassword { get; set; }

    public static string AdministratorsEmails { get; set; }

    public static int TokenLinkRedefinePasswordExpiredLimitMinutesTime
    {
        get
        {
            var limitExpiredTImeString = WebConfigurationManager.AppSettings.Get("TokenLinkRedefinePasswordExpiredLimitMinutesTime");
            int limitExpiredTime = 0;
            bool isParseOk = Int32.TryParse(limitExpiredTImeString, out limitExpiredTime);

            return limitExpiredTime;
        }
    }
    public static List<string> AdministratorsEmails_List
    {
        get
        {
            var list = new List<string>();

            foreach (var address in AdministratorsEmails.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                list.Add(address);
            }

            return list;
        }
    }
    public static object BaseUrl { get; set; }

    static Settings()
    {
        MesJolisCotillonsGmailUserName = WebConfigurationManager.AppSettings.Get("MesJolisCotillonsGmailUserName");
        MesJolisCotillonsGmailPassword = WebConfigurationManager.AppSettings.Get("MesJolisCotillonsGmailPassword");
        AdministratorsEmails = WebConfigurationManager.AppSettings.Get("AdministratorsEmails");
        //TokenLinkRedefinePasswordExpiredLimitMinutesTime = WebConfigurationManager.AppSettings.Get("TokenLinkRedefinePasswordExpiredLimitMinutesTime");
        //BaseUrl = WebConfigurationManager.AppSettings.Get("BaseUrl");
    }
}