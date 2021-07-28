using MesJolisCotillons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Net;
using System.Net.Mail;
using MesJolisCotillons.Extensions;

namespace MesJolisCotillons.Area
{
    public class LoginController : Controller
    {
        User_Repository user_Repository = new User_Repository();


        public ActionResult Index()
        {
            if (CurrentUser.User != null)
            {
                return Redirect("/");
            }
            return View("~/Views/Login/Login.cshtml");
        }

        [AjaxOrChildActionOnly]
        [HttpPost]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return Json(new
            {
                success = true
            });
        }

        [AjaxOrChildActionOnly]
        [HttpPost]
        public ActionResult InscriptionCustomer(LoginController.Customer_UserForm customerForm)
        {
            var user = user_Repository.FindCustomer_UserByEmail(customerForm.eMail);

            if (user != null && user.HasBeenAlreadySuscribed)
            {
                return Json(new
                {
                    success = false,
                    msg = LoginController.InscriptionCustomerMessage.eMailAlreadyExists
                });
            }


            if (String.IsNullOrEmpty(customerForm.eMail) || String.IsNullOrEmpty(customerForm.FirstName) ||
                String.IsNullOrEmpty(customerForm.LastName) || String.IsNullOrEmpty(customerForm.Password) ||
                String.IsNullOrEmpty(customerForm.ConfirmedPassword))
            {
                return Json(new
                {
                    success = false,
                    msg = LoginController.InscriptionCustomerMessage.MissingMandatoryFields
                });
            }

            if (customerForm.ConfirmedPassword.Length < 6 || customerForm.Password.Length < 6)
            {
                return Json(new
                {
                    success = false,
                    msg = LoginController.InscriptionCustomerMessage.PasswordDoNotHaveMinimumCharacters
                });

            }

            if (customerForm.Password != customerForm.ConfirmedPassword)
            {
                return Json(new
                {
                    success = false,
                    msg = LoginController.InscriptionCustomerMessage.PasswordAndConfirmedPasswordNotMatching
                });
            }


            Customer_User customerUserFromForm = user_Repository.createCustomer_UserFromForm(customerForm, existingUser: user);
            user_Repository.Save();

            string leftPart = this.Request.Url.GetLeftPart(UriPartial.Authority);

            this.sendInscriptionEmailConfirmation(customerUserFromForm, leftPart);
            user_Repository.Save();

            return Json(new
            {
                success = true,
                msg = LoginController.InscriptionCustomerMessage.UserCreatedAwaitingMailConfirmation
            });
        }


        [AjaxOrChildActionOnly]
        [HttpPost]
        public ActionResult EmailPasswordForRedefinition(string eMail)
        {
            var customerUser = user_Repository.FindCustomer_UserByEmail(eMail, Customer_User_Status.Active);

            if (customerUser == null)
            {
                return Json(new
                {
                    success = false,
                    msg = "L'email fourni ne semble pas correspondre à un utilisateur du site"
                });
            }

            string leftPart = this.Request.Url.GetLeftPart(UriPartial.Authority);
            sendPasswordRedefinitionEmail(customerUser, leftPart);

            user_Repository.Save();

            return Json(new
            {
                success = true,
                msg = "Un mail a été envoyé avec succès"
            });
        }

        #region LoginAjax (commented)
        //[AjaxOrChildActionOnly]
        //[HttpPost]
        //public ActionResult LoginAjax(User userModel, string ReturnUrl)
        //{
        //    var user = user_Repository.authenticate(userModel);
        //    if (user != null)
        //    {
        //        FormsAuthentication.SetAuthCookie(user.User_ID.ToString(), false);
        //        if (!string.IsNullOrWhiteSpace(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
        //        {
        //            return Json(new
        //            {
        //                ReturnUrl = ReturnUrl,
        //                success = true
        //            });
        //        }

        //        return Json(new
        //        {
        //            ReturnUrl = "/",
        //            success = true
        //        });
        //    }

        //    return Json(new
        //    {
        //        msg = "Email et Mot de passe ne semblent pas correspondre",
        //        success = false
        //    });
        //} 
        #endregion

        [AjaxOrChildActionOnly]
        [HttpPost]
        public ActionResult CheckLoginPassword(User userModel)
        {
            var customerUser = user_Repository.authenticateCustomerUser(userModel);
            if (customerUser != null)
            {
                return Json(new
                {
                    msg = "Authentification Ok",
                    success = true
                });
            }

            return Json(new
            {
                msg = "Email et Mot de passe ne semblent pas correspondre",
                success = false
            });
        }


        [AjaxOrChildActionOnly]
        [HttpPost]
        public ActionResult LoginAjax(User userModel)
        {
            var customerUser = user_Repository.authenticateCustomerUser(userModel);
            if (customerUser != null)
            {
                FormsAuthentication.SetAuthCookie(customerUser.User_FK.ToString(), false);
                return Json(new
                {
                    msg = "Vous êtes connecté " + customerUser.User.FullName + "(" + customerUser.User.eMail + ")",
                    success = true
                });
            }

            return Json(new
            {
                msg = "Email et Mot de passe ne semblent pas correspondre",
                success = false
            });
        }

        #region View

        [HttpPost]
        public ActionResult LoginView(User userModel, string ReturnUrl)
        {
            var customerUser = user_Repository.authenticateCustomerUser(userModel);
            if (customerUser != null)
            {
                FormsAuthentication.SetAuthCookie(customerUser.User_FK.ToString(), false);
                if (!string.IsNullOrWhiteSpace(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }

                return Redirect("/");
            }
            return View("~/Views/Login/Login.cshtml");
        }

        public ActionResult InscriptionFormView()
        {
            if (CurrentUser.User != null)
            {
                return Redirect("/");
            }

            return View("~/Views/Login/InscriptionFormView.cshtml");
        }

        public ActionResult PasswordForgottenView()
        {
            return View("~/Views/Login/PasswordForgotten.cshtml");
        }
        #endregion

        #region Private methods, Class, enum
        private void sendInscriptionEmailConfirmation(Customer_User customerUser, string baseUrl)
        {
            string inscriptionTokenLink = customerUser.generateInscriptionTokenLink();
            string eMail = customerUser.User.eMail;

            string bodyContent = "<h4>Bonjour, </h4><br>" + "Nous vous remercions pour votre inscription sur le site de Mes Jolis Cotillons.<br><br>" + "<div>Nous vous invitons à cliquer sur le lien ci-dessous afin de valider votre inscription:</div>" + "<br><a href='" + baseUrl + "//Customer//AccountValidation//" + inscriptionTokenLink + "' > Cliquez-ici</a><br>" + "<br><div>A très bientôt sur notre site !</div><br>" + "<div><b>L'équipe de Mes Jolis Cotillons</b></div><br><br>";

            Util.sendEmail(bodyContent, "Mes Jolis Cotillons - Confirmation inscription", eMail);
        }

        private void sendPasswordRedefinitionEmail(Customer_User customerUser, string baseUrl)
        {

            string redefinePasswordTokenLink = customerUser.generatePasswordRedefinitionTokenLink();
            string eMail = customerUser.User.eMail;
            var limitLinkExpiration = Settings.TokenLinkRedefinePasswordExpiredLimitMinutesTime;

            string bodyContent = "<h4>Bonjour, </h4><br>" + "Une demande de modification de mot de passe sur votre compte a été effectuée.<br>" + "<div>Nous vous invitons à cliquer sur le lien ci-dessous afin de le réinitialiser (Le lien est valable " + limitLinkExpiration + " minutes).</div>" + "<br><a href='" + baseUrl + "//Customer//PasswordRedefinitionView//" + redefinePasswordTokenLink + "' > Cliquez-ici</a><br>" + "<br><div>Pour toute autre question, n'hésitez pas à nous contacter.</div><br><div> A bientôt sur le site,</div><br>" + "<div><b>L'équipe mes jolis cotillons</b></div><br><br>";

            Util.sendEmail(bodyContent, "Mes Jolis Cotillons - Redéfinition de mot de passe", eMail);
        }

        public class Customer_UserForm
        {
            private string _Country = "France";
            private string _eMail;
            private string _Password;
            private string _ConfirmedPassword;
            private string _FirstName;
            private string _LastName;
            private string _Address;
            private string _ZipCode;
            private string _City;
            private string _Phone;

            public string eMail
            {
                get
                {
                    return _eMail;
                }
                set
                {
                    _eMail = value != null ? value.Trim() : null;
                }
            }

            public string Password
            {
                get
                {
                    return _Password;
                }
                set
                {
                    _Password = value != null ? value.Trim() : null;
                }
            }

            public string ConfirmedPassword
            {
                get
                {
                    return _ConfirmedPassword;
                }
                set
                {
                    _ConfirmedPassword = value != null ? value.Trim() : null;
                }
            }

            public string FirstName
            {
                get
                {
                    return _FirstName;
                }
                set
                {
                    _FirstName = value != null ? value.Trim() : null;
                }
            }

            public string LastName
            {
                get
                {
                    return _LastName;
                }
                set
                {
                    _LastName = value != null ? value.Trim() : null;
                }
            }

            public string Address
            {
                get
                {
                    return _Address;
                }
                set
                {
                    _Address = value != null ? value.Trim() : null;
                }
            }

            public string ZipCode
            {
                get
                {
                    return _ZipCode;
                }
                set
                {
                    _ZipCode = value != null ? value.Trim() : null;
                }
            }

            public string City
            {
                get
                {
                    return _City;
                }
                set
                {
                    _City = value != null ? value.Trim() : null;
                }
            }

            public string Country
            {
                get
                {
                    return _Country;
                }
                set
                {
                    _Country = value != null ? value.Trim() : null;
                }
            }

            public string Phone
            {
                get
                {
                    return _Phone;
                }
                set
                {
                    _Phone = value != null ? value.Trim() : null;
                }
            }

            public int? User_ID { get; set; }
        }

        public enum InscriptionCustomerMessage
        {
            eMailAlreadyExists = -300,
            PasswordAndConfirmedPasswordNotMatching = -200,
            PasswordDoNotHaveMinimumCharacters = -50,
            MissingMandatoryFields = -100,
            UserCreatedAwaitingMailConfirmation = 100,
        }
        #endregion
    }
}