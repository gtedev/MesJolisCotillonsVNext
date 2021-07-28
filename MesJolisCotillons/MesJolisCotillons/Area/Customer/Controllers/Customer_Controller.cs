using MesJolisCotillons.Extensions.Security;
using MesJolisCotillons.Models;
using MesJolisCotillons.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace MesJolisCotillons.Area.Customer.Controllers
{
    public class CustomerController : Controller
    {
        private User_Repository user_Repository = new User_Repository();
        private Util_Repository util_Repository = new Util_Repository();
        private Command_Repository command_Repository = new Command_Repository();

        [CustomerAuthorize]
        public ActionResult Index()
        {

            var customerUserModel = new CustomerAccountViewModel()
            {
                itemSelectedId = "CustomerAccountEmailPassword"
            };

            return (ActionResult)this.View("~/Views/Customer/CustomerAccount.cshtml", customerUserModel);
        }

        [AjaxOrChildActionOnly]
        public ActionResult getCurrentCustomerUser()
        {
            var customer_User = CurrentUser.User?.Customer_User;

            return Json(new
            {
                success = true,
                isAuthenticated = customer_User != null,
                CurrentUser = customer_User == null ? null : new
                {
                    CurrentUser.User.FullName,
                    CurrentUser.User.User_ID
                }
            });
        }



        [AjaxOrChildActionOnly]
        [CustomerAuthorize]
        public ActionResult editCustomerAccountParameters(LoginController.Customer_UserForm customerForm)
        {
            Customer_User customerUser = user_Repository.FindCustomer_User(CurrentUser.User.User_ID);


            if (String.IsNullOrEmpty(customerForm.eMail) || String.IsNullOrEmpty(customerForm.FirstName) ||
                String.IsNullOrEmpty(customerForm.LastName))
            {
                return Json(new
                {
                    success = false,
                    msg = "Veuillez remplir les champs requis correctement"
                });
            }



            if (!String.IsNullOrEmpty(customerForm.Password) ||
                !String.IsNullOrEmpty(customerForm.ConfirmedPassword))
            {
                if (customerForm.ConfirmedPassword.Length < 6 || customerForm.Password.Length < 6)
                {
                    return Json(new
                    {
                        success = false,
                        msg = "Les mots de passent doivent avoir un minimum de 6 caractères"
                    });
                }

                if (customerForm.Password != customerForm.ConfirmedPassword)
                {
                    return Json(new
                    {
                        success = false,
                        msg = "Les mots de passent ne correspondent pas"
                    });
                }
            }

            customerUser.User.FirstName = customerForm.FirstName;
            customerUser.User.LastName = customerForm.LastName;



            if (!String.IsNullOrEmpty(customerForm.Password) &&
                !String.IsNullOrEmpty(customerForm.ConfirmedPassword) && customerForm.Password == customerForm.ConfirmedPassword
                && customerForm.ConfirmedPassword.Length >= 6 && customerForm.Password.Length >= 6)
            {
                customerUser.setSaltHashedSHA256Password(customerForm.Password);
            }


            if (!String.IsNullOrEmpty(customerForm.Phone))
            {
                customerUser.setCustomerPhone(customerForm.Phone);
            }


            user_Repository.Save();

            return (ActionResult)this.Json(new
            {
                success = true,
                msg = "Votre compte a été édité avec succès"
            });
        }

        [CustomerAuthorize]
        [AjaxOrChildActionOnly]
        private ActionResult editCustomerAccountAddress(CustomerAccountViewModel customerForm, CustomerAddress address, Customer_User customerUser)
        {
            //Customer_User customerUser = this.user_Repository.FindCustomer_User(CurrentUser.User.User_ID);
            //Address address = customerUser.Address_Set.FirstOrDefault();

            //if (address == null)
            //{
            //    address = new Address();
            //}

            if (!String.IsNullOrEmpty(customerForm.Address))
            {
                address.Address.Address1 = customerForm.Address;
            }

            if (!String.IsNullOrEmpty(customerForm.City))
            {
                address.Address.City = customerForm.City;
            }

            if (!String.IsNullOrEmpty(customerForm.ZipCode))
            {
                address.Address.Zip_Code = customerForm.ZipCode;
            }

            int countryId;
            if (!String.IsNullOrEmpty(customerForm.Country) && Int32.TryParse(customerForm.Country, out countryId))
            {

                bool isParseOk = Int32.TryParse(customerForm.Country, out countryId);
                var country = util_Repository.FindCountry(countryId);
                if (country != null)
                {
                    //address.Country_FK = countryId;
                    address.Address.Country = country;
                }
            }

            customerUser.CustomerAddress_Set.Add(address);


            this.user_Repository.Save();

            return Json(new
            {
                success = true,
                msg = "Votre compte a été édité avec succès"
            });
        }

        [AjaxOrChildActionOnly]
        [CustomerAuthorize]
        public ActionResult editCustomerAccountAddressShipment(CustomerAccountViewModel customerForm)
        {
            Customer_User customerUser = this.user_Repository.FindCustomer_User(CurrentUser.User.User_ID);
            CustomerAddress shipmentAddress = customerUser.CustomerAddressShipment;

            if (shipmentAddress == null)
            {
                shipmentAddress = new CustomerAddress
                {
                    Address = new Address(),
                    Type = CustomerAddress_Type.Shipment
                };
            }

            return editCustomerAccountAddress(customerForm, shipmentAddress, customerUser);
        }

        [AjaxOrChildActionOnly]
        [CustomerAuthorize]
        public ActionResult editCustomerAccountAddressInvoice(CustomerAccountViewModel customerForm)
        {
            Customer_User customerUser = this.user_Repository.FindCustomer_User(CurrentUser.User.User_ID);
            CustomerAddress shipmentAddress = customerUser.CustomerAddressInvoice;

            if (shipmentAddress == null)
            {
                shipmentAddress = new CustomerAddress
                {
                    Address = new Address(),
                    Type = CustomerAddress_Type.Invoice
                };
            }

            return editCustomerAccountAddress(customerForm, shipmentAddress, customerUser);
        }

        [AjaxOrChildActionOnly]
        [HttpPost]
        public ActionResult redefinePassword(string tokenLink, string password, string confirmedPassword)
        {
            var customerUser = this.user_Repository.FindAllCustomer_User()
                                                 .AsEnumerable()
                                                 .Where(item => item.isRedefiningPasswordTokenStillValid(tokenLink))
                                                 .FirstOrDefault();

            if (customerUser == null)
            {
                return Json(new
                {
                    success = false,
                    msg = "Le lien ne semble plus utilisable"
                });
            }

            if (password.Length < 6 || confirmedPassword.Length < 6)
            {
                return Json(new
                {
                    success = false,
                    msg = "Les mot de passes doivent avoir au moins 6 caractères"
                });

            }

            if (password != confirmedPassword)
            {
                return Json(new
                {
                    success = false,
                    msg = "Les mots de passe ne correspondent pas"
                });
            }

            customerUser.setSaltHashedSHA256Password(password);
            var isSetStatusSuccess = customerUser.setRedefinedPasswordProcessStatus(RedefinePasswordProcessStatus.passwordRedefined, tokenLink);

            if (!isSetStatusSuccess)
            {
                throw new Exception("redefinePassword: Problem in setRedefinedPasswordProcessStatus method");
            }

            user_Repository.Save();

            return Json(new
            {
                success = true,
                msg = "Les mot de passe a été correctement changée"
            });
        }

        [HttpGet]
        public ActionResult CheckTokenLinkRedefinePasswordExpiration()
        {
            try
            {
                var datetimeNow = DateTime.Now;
                int minutesTimeLimit = Settings.TokenLinkRedefinePasswordExpiredLimitMinutesTime;
                int countTokenLinkExpired = 0;

                #region Delegate functions
                Func<RedefinitionPasswordProcess, bool> isTokenLinkExpired = (process) =>
                {
                    if (process == null)
                    {
                        return false;
                    }

                    //if (process.Status > RedefinePasswordProcessStatus.emailSent)
                    //{
                    //    return false;
                    //}

                    var processCreatedHistory = process.RedefinePasswordProcessActionHistory.Where(item => item.Status == RedefinePasswordProcessStatus.emailSent).FirstOrDefault();
                    if (processCreatedHistory == null)
                    {
                        return false;
                    }

                    var isMinutesTimeLimitExpired = (datetimeNow - processCreatedHistory.Date).TotalMinutes >= minutesTimeLimit;

                    return isMinutesTimeLimitExpired;
                };

                Action<Customer_User> handleCustomerRedefineProcess = (customerUser) =>
                {
                    if (customerUser == null)
                    {
                        return;
                    }

                    var customerXmlData = customerUser.Customer_XmlData;
                    var redefineProcessStillAwaitingAction_Set = customerXmlData.RedefinitionPasswordProcess_Set.Where(item => item.Status == RedefinePasswordProcessStatus.emailSent)
                                                                                                                .ToList();
                    foreach (var item in redefineProcessStillAwaitingAction_Set)
                    {
                        if (isTokenLinkExpired(item))
                        {
                            item.Status = RedefinePasswordProcessStatus.tokenExpired;
                            countTokenLinkExpired++;
                        }
                    }

                    customerUser.xmlData = customerXmlData.Serialize();
                };
                #endregion

                var customer_Set = user_Repository.FindAllCustomer_User()
                                                        .AsEnumerable()
                                                        .Where(item => item.Customer_XmlData.RedefinitionPasswordProcess_Set.Any(a => a.Status == RedefinePasswordProcessStatus.emailSent));

                var count = customer_Set.Count();

                foreach (var item in customer_Set)
                {
                    handleCustomerRedefineProcess(item);
                }

                if (countTokenLinkExpired >= 1)
                {
                    user_Repository.Save();
                }


                return Json(new
                {
                    HttpStatusCode = HttpStatusCode.OK,
                    message = "CheckTokenLinkRedefinePasswordExpiration: " + countTokenLinkExpired + " tokenlink(s) de mot de passe à redéfinir expirée(s) car la limite (" + minutesTimeLimit + " minute(s)) a été dépassée."
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    message = "CheckTokenLinkRedefinePasswordExpiration: InnerException: " + e.InnerException
                }, JsonRequestBehavior.AllowGet);

            }
        }

        [AjaxOrChildActionOnly]
        [HttpPost]
        public ActionResult isEmailAlreadyUsed(string email)
        {
            if (String.IsNullOrEmpty(email) || !Util.IsValidEmail(email))
            {
                return (ActionResult)this.Json(new
                {
                    success = false,
                    msg = "Le mail fournit n'est pas valide"
                });
            }


            var user = user_Repository.FindCustomer_UserByEmail(email);
            if (user != null && user.HasBeenAlreadySuscribed)
            {
                return (ActionResult)this.Json(new
                {
                    success = false,
                    msg = "Ce mail est déjà utilisé"
                });
            }


            return (ActionResult)this.Json(new
            {
                success = true,
                msg = "Ce mail peut être utilisé"
            });

        }

        #region PartialView

        [AjaxOrChildActionOnly]
        //[CustomerAuthorize]
        public ActionResult Customer_AddressBasePartialView(CustomerAccountViewModel customerModel, int? CustomerUser_FK, int? Address_FK = null)
        {
            CustomerAccountViewModel addressModel = new CustomerAccountViewModel();
            CustomerAddress customerAddress = null;

            var countryViewModel_Set = this.util_Repository.FindAllCountry()
                                                           .Select(item => new CountryViewModel
                                                           {
                                                               Name = item.Name,
                                                               Country_FK = item.Country_ID
                                                           }).AsEnumerable();

            addressModel.Country_Set = countryViewModel_Set;

            if (customerModel.CustomerUser_FK != null)
            {
                var customer = user_Repository.FindCustomer_User((int)CustomerUser_FK);
                if (customer != null)
                {
                    addressModel.FirstName = customer.User.FirstName;
                    addressModel.LastName = customer.User.LastName;
                    if (customerModel.Address_FK != null)
                    {
                        customerAddress = customer.CustomerAddress_Set.Where(item => item.Address_FK == Address_FK)
                                                        .FirstOrDefault();
                    }
                    else
                    {
                        customerAddress = customer.CustomerAddress_Set.FirstOrDefault();
                    }

                    addressModel.Address = customerAddress?.Address?.Address1;
                    addressModel.City = customerAddress?.Address?.City;
                    addressModel.ZipCode = customerAddress?.Address?.Zip_Code;
                    addressModel.Country = customerAddress?.Address?.Country?.Name;
                    addressModel.Country_FK = customerAddress?.Address?.Country?.Country_ID;
                }
            }

            return PartialView("~/Views/Customer/_CustomerAddressBase.cshtml", addressModel);
        }

        [AjaxOrChildActionOnly]
        public ActionResult CustomerInscriptionFirstStepValidationPartialView()
        {
            return PartialView("~/Views/Customer/CustomerInscriptionFirstStepValidation.cshtml");
        }

        [AjaxOrChildActionOnly]
        public ActionResult CustomerRedefinePasswordValidationPartialView()
        {
            return PartialView("~/Views/Customer/CustomerRedefinePasswordValidation.cshtml");
        }


        [CustomerAuthorize]
        [AjaxOrChildActionOnly]
        public ActionResult CustomerAccountAddressShipmentFormPartialView()
        {
            CustomerAddress address = user_Repository.FindCustomer_User(CurrentUser.User.User_ID)
                                             .CustomerAddressShipment;

            CustomerAccountViewModel accountViewModel = new CustomerAccountViewModel
            {
                Address = address?.Address?.Address1,
                City = address?.Address?.City,
                ZipCode = address?.Address?.Zip_Code,
                Country = address?.Address?.Country?.Name,
                Country_FK = address?.Address?.Country?.Country_ID,
                CustomerUser_FK = CurrentUser.User.User_ID,
                FirstName = CurrentUser.User.FirstName,
                LastName = CurrentUser.User.LastName,
                itemSelectedId = "CustomerAccountPersonalDataForm"
            };

            return (ActionResult)this.PartialView("~/Views/Customer/CustomerAccountAddressShipmentForm.cshtml", accountViewModel);
            //return PartialView("~/Views/Customer/_CustomerAddressBase.cshtml", accountViewModel);
        }

        [CustomerAuthorize]
        [AjaxOrChildActionOnly]
        public ActionResult CustomerAccountAddressInvoiceFormPartialView()
        {
            CustomerAddress address = user_Repository.FindCustomer_User(CurrentUser.User.User_ID)
                                             .CustomerAddress_Set
                                             .Where(item => item.Type != null && ((CustomerAddress_Type)item.Type).HasFlag(CustomerAddress_Type.Invoice))
                                             .FirstOrDefault();

            CustomerAccountViewModel accountViewModel = new CustomerAccountViewModel
            {
                Address = address?.Address?.Address1,
                City = address?.Address?.City,
                ZipCode = address?.Address?.Zip_Code,
                Country = address?.Address?.Country?.Name,
                Country_FK = address?.Address?.Country?.Country_ID,
                CustomerUser_FK = CurrentUser.User.User_ID,
                FirstName = CurrentUser.User.FirstName,
                LastName = CurrentUser.User.LastName,
                itemSelectedId = "CustomerAccountAddressInvoiceForm"
            };

            return (ActionResult)this.PartialView("~/Views/Customer/CustomerAccountAddressInvoiceForm.cshtml", accountViewModel);
            //return PartialView("~/Views/Customer/_CustomerAddressBase.cshtml", accountViewModel);
        }

        [CustomerAuthorize]
        [AjaxOrChildActionOnly]
        public ActionResult CustomerAccountEmailPasswordPartialView()
        {
            Customer_User customerUser = this.user_Repository.FindCustomer_User(CurrentUser.User.User_ID);

            var customerUserModel = new CustomerAccountViewModel()
            {
                eMail = customerUser?.User?.eMail,
                FirstName = customerUser?.User?.FirstName,
                LastName = customerUser?.User?.LastName,
                itemSelectedId = "CustomerAccountEmailPassword",
                Phone = customerUser?.Customer_XmlData?.Phone
            };

            return PartialView("~/Views/Customer/CustomerAccountEmailPassword.cshtml", customerUserModel);
        }

        [CustomerAuthorize]
        [AjaxOrChildActionOnly]
        public ActionResult CustomerAccountCommandListPartialView()
        {
            Customer_User customerUser = this.user_Repository.FindCustomer_User(CurrentUser.User.User_ID);
            var listCommand = customerUser.Command_Set.Where(item => item.Command_Status >= Command_Status.Command_Paid)
                                                      .OrderByDescending(item => item.Command_ID)
                                                      .ToList();

            var customerUserModel = new CustomerAccountViewModel()
            {
                eMail = customerUser?.User?.eMail,
                FirstName = customerUser?.User?.FirstName,
                LastName = customerUser?.User?.LastName,
                Command_Set = listCommand
            };

            return PartialView("~/Views/Customer/CustomerAccountCommandList.cshtml", customerUserModel);
        }

        [CustomerAuthorize]
        [AjaxOrChildActionOnly]
        public ActionResult CommandDetails(string id)
        {
            int Command_ID = -1;
            var isParseOk = Int32.TryParse(id, out Command_ID);

            if (!isParseOk)
            {
                throw new Exception("CommandDetails: problem to parse value" + id);
            }
            var command = command_Repository.FindCommand(Command_ID);

            if (command == null)
            {
                throw new Exception("CommandDetails: Command avec Command_ID " + id + " + ne semble plus exister");
            }
            return PartialView("~/Views/Customer/CommandDetails.cshtml", command);
        }
        #endregion

        #region View

        public ActionResult PasswordRedefinitionView(string id)
        {
            RedefinitionPasswordProcess redefineProcess = this.user_Repository.FindAllCustomer_User()
                                                             .AsEnumerable()
                                                             .SelectMany(item => item.Customer_XmlData.RedefinitionPasswordProcess_Set)
                                                             .Where(item => item.TokenLinkGenerated == id)
                                                             .FirstOrDefault();


            if (redefineProcess == null)
            {
                return (ActionResult)this.Redirect("/");
            }

            if (redefineProcess.Status == RedefinePasswordProcessStatus.tokenExpired || redefineProcess.Status == RedefinePasswordProcessStatus.passwordRedefined)
            {
                return (ActionResult)this.View("~/Views/Customer/CustomerTokenLinkExpired.cshtml");
            }

            var model = new TokenLinkProcessModel
            {
                TokenLinkGenerated = redefineProcess.TokenLinkGenerated
            };

            return (ActionResult)this.View("~/Views/Customer/CustomerRedefinitionPasswordForm.cshtml", model);
        }

        public ActionResult AccountValidation(string id)
        {
            Customer_User customerUser = this.user_Repository.FindAllCustomer_User()
                                                             .Where(item => item.Status == Customer_User_Status.Awaiting_MailConfirmation)
                                                             .AsEnumerable()
                                                             .Where(item => item.InscriptionTokenLink == id && !item.IsIncriptionLinkConfirmed)
                                                             .FirstOrDefault();

            if (customerUser == null)
            {
                return (ActionResult)this.Redirect("/");
            }


            customerUser.SetInscriptionUserConfirmed();
            this.user_Repository.Save();

            return (ActionResult)this.View("~/Views/Customer/CustomerConfirmation.cshtml");
        }

        [CustomerAuthorize]
        public ActionResult CustomerAccountCommandListView()
        {
            Customer_User customerUser = this.user_Repository.FindCustomer_User(CurrentUser.User.User_ID);
            if (customerUser == null)
            {
                return Redirect("/");
            }

            var customerUserModel = new CustomerAccountViewModel()
            {
                eMail = customerUser?.User?.eMail,
                FirstName = customerUser?.User?.FirstName,
                LastName = customerUser?.User?.LastName,
                itemSelectedId = "CustomerAccountCommandList"
            };

            return View("~/Views/Customer/CustomerAccount.cshtml", customerUserModel);
        }

        [AjaxOrChildActionOnly]
        public ActionResult IsCustomerUserConnected()
        {
            var isCustomerUserConnected = false;
            var customerUserName = "";
            int? customerId = null;

            if (CurrentUser.User != null)
            {
                customerUserName = CurrentUser.User?.FullName;
                isCustomerUserConnected = true;
                customerId = CurrentUser.User?.User_ID;
            }

            return Json(new
            {
                success = true,
                isCustomerUserConnected = isCustomerUserConnected,
                Customer_User_ID = customerId
            }, JsonRequestBehavior.AllowGet);
        }


        [CustomerAuthorize]
        public ActionResult CustomerAccountAddressShipmentFormView()
        {
            var customerUserModel = new CustomerAccountViewModel()
            {
                itemSelectedId = "CustomerAccountAddressShipmentForm"
            };

            return View("~/Views/Customer/CustomerAccount.cshtml", customerUserModel);
        }

        [CustomerAuthorize]
        public ActionResult CustomerAccountAddressInvoiceFormView()
        {
            var customerUserModel = new CustomerAccountViewModel()
            {
                itemSelectedId = "CustomerAccountAddressInvoiceForm"
            };

            return View("~/Views/Customer/CustomerAccount.cshtml", customerUserModel);
        }

        [CustomerAuthorize]
        public ActionResult CustomerAccountEmailPasswordView()
        {
            var customerUserModel = new CustomerAccountViewModel()
            {
                itemSelectedId = "CustomerAccountEmailPassword"
            };

            return View("~/Views/Customer/CustomerAccount.cshtml", customerUserModel);
        }
    }

    #endregion

}