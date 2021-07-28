using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;


public static class Util
{
    public static string RenderViewToString(string viewPath, object Model)
    {
        var result = "";
        try
        {
            var IsTemplateCached = Engine.Razor.IsTemplateCached(viewPath, Model.GetType());
            if (!IsTemplateCached)
            {
                //var templateString = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(viewPath));
                var templateString = File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath(viewPath));
                result = Engine.Razor.RunCompile(templateString, viewPath, Model.GetType(), Model);
            }
            else
            {
                var template = Engine.Razor.GetKey(viewPath);
                result = Engine.Razor.Run(template, Model.GetType(), Model);
            }
        }
        catch (Exception e)
        {
            throw e;
        }

        return result;
    }

    public static void sendEmail(string bodyContent, string subjectEmail, string emailTo)
    {
        var list = new List<string> { emailTo };
        sendEmail(bodyContent, subjectEmail, list);
    }
    public static void sendEmail(string bodyContent, string subjectEmail, List<string> emailTo_List)
    {
        try
        {
            string body = bodyContent;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = (ICredentialsByHost)new NetworkCredential(Settings.MesJolisCotillonsGmailUserName, Settings.MesJolisCotillonsGmailPassword);

            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress(Settings.MesJolisCotillonsGmailUserName);
                message.Subject = subjectEmail;
                message.Body = body;
                message.IsBodyHtml = true;

                //var addresses = Settings.AdministratorsEmails;
                foreach (var emailAddress in emailTo_List)
                {
                    if (!String.IsNullOrEmpty(emailAddress))
                        message.To.Add(emailAddress.Trim());
                }

                smtpClient.Send(message);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("L'application a rencontrée l'erreur suivante : " + Environment.NewLine + ex.Message.ToString());
        }
    }
    public static bool IsValidEmail(string email)
    {
        string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                                  + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                                  + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

        var regex = new Regex(validEmailPattern, RegexOptions.IgnoreCase);
        bool isValid = regex.IsMatch(email);

        return isValid;
    }
}
