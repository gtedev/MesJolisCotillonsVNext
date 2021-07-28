using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MesJolisCotillons.Area.Information.Controllers
{
    //[OutputCache(Duration = 3600)]
    public class InformationController : Controller
    {
        #region View
        public ActionResult NeedHelpView()
        {
            return View("~/Views/Information/About.cshtml");
        }

        public ActionResult AboutView()
        {
            return View("~/Views/Information/About.cshtml");
        }

        public ActionResult QuestionsAnswersView()
        {
            return View("~/Views/Information/QuestionsAnswers.cshtml");
        }

        public ActionResult GeneralSalesConditionsView()
        {
            return View("~/Views/Information/GeneralSalesConditions.cshtml");
        }

        public ActionResult LegalNoticesView()
        {
            return View("~/Views/Information/LegalNotices.cshtml");
        }

        //[AjaxOrChildActionOnly]
        public ActionResult CannotHandleMobile()
        {
            return View("~/Views/Information/CannotHandleMobile.cshtml");
        }

        public ActionResult Contact()
        {
            ContactForm modelForm = new ContactForm
            {
                Email = "",
                FirstName = "",
                LastName = "",
                Phone = ""
            };
            if (CurrentUser.User != null)
            {
                modelForm.Email = CurrentUser.User.eMail;
                modelForm.FirstName = CurrentUser.User.FirstName;
                modelForm.LastName = CurrentUser.User.LastName;

                if (!String.IsNullOrEmpty(CurrentUser.User?.Customer_User?.Customer_XmlData?.Phone))
                {
                    modelForm.Phone = CurrentUser.User.Customer_User.Customer_XmlData.Phone;
                }

            }
            return View("~/Views/Information/Contact.cshtml", modelForm);
        }
        #endregion

        #region PartialView
        //[AjaxOrChildActionOnly]
        //public ActionResult AboutPartialView()
        //{
        //    return PartialView("~/Views/Footer/About.cshtml");
        //}

        //[AjaxOrChildActionOnly]
        //public ActionResult FrequentlyAskedQuestionsPartialView()
        //{
        //    return PartialView("~/Views/Footer/FrequentlyAskedQuestions.cshtml");
        //}

        //[AjaxOrChildActionOnly]
        //public ActionResult GeneralSalesConditionsPartialView()
        //{
        //    return PartialView("~/Views/Footer/GeneralSalesConditions.cshtml");
        //}

        //[AjaxOrChildActionOnly]
        //public ActionResult LegalNoticesPartialView()
        //{
        //    return PartialView("~/Views/Footer/LegalNotices.cshtml");
        //} 
        #endregion

        [AjaxOrChildActionOnly]
        public ActionResult SendContactFormMessage(ContactForm contactForm)
        {
            if (String.IsNullOrEmpty(contactForm.FirstName) || String.IsNullOrEmpty(contactForm.LastName) || String.IsNullOrEmpty(contactForm.Email) || String.IsNullOrEmpty(contactForm.Object) || String.IsNullOrEmpty(contactForm.Message))
            {
                return Json(new
                {
                    success = false,
                    msg = "Veuillez remplir les champs requis"
                });
            }

            sendContactFormMessageToAdministrators(contactForm);

            return Json(new
            {
                success = true,
                msg = "Le message a bien été envoyé"
            });
        }

        private void sendContactFormMessageToAdministrators(ContactForm contactForm)
        {
            #region subjectEmail
            var subjectEmail = contactForm.Object;
            #endregion

            #region bodyContent
            string bodyContent = "<h4>Bonjour, </h4><br>" + "Vous avez reçu un message d'un visiteur via le formulaire de Contact sur le site Mes Jolis Cotillons.<br>" +
              "<div>Voici le message ci-dessous:</div>" + "<br>" + "<br>" +

              "<div><b>Email:</b> " + contactForm.Email + "</div>" +
              "<div><b>Prénom et Nom:</b> " + contactForm.FirstName + " " + contactForm.LastName + "</div>" +
              "<div><b>Téléphone:</b> " + contactForm.Phone + "</div>" + "<br>" +
              "<div><b>Objet:</b> " + contactForm.Object + "</div>" + "<br>" +
              "<div><b>Message:</b> </div><br>" + contactForm.Message + "<br>" +

              "<br><br>A très bientôt, <br><br><b>Le robot Mes Jolis Cotillons</b><br>";

            #endregion

            var emailAdministratorsList = Settings.AdministratorsEmails_List;
            Util.sendEmail(bodyContent, "Mes Jolis Cotillons - Message d'un visiteur via la page Contact", emailAdministratorsList);
        }

        public class ContactForm
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Object { get; set; }
            public string Phone { get; set; }
            public string Message { get; set; }
        }
    }
}