using Ext.Direct.Mvc;
using MesJolisCotillons.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static MesJolisCotillons.Area.LoginController;

namespace MesJolisCotillons.Area._Admin.Controllers.Direct
{
    [AdminAuthorize]
    [DirectHandleError]
    public class UserDirectController : DirectController
    {
        User_Repository user_Repository = new User_Repository();

        public ActionResult getUser_Set(DirectStoreQuery q)
        {
            var user_Set = user_Repository.FindAllUser().Where(item => item.Customer_User != null && item.Customer_User.Status == Customer_User_Status.Active);


            #region orderby
            user_Set = user_Set.OrderBy(item => item.User_ID);
            //.OrderBy(item => item.LastName);
            //.ThenBy(item => item.FirstName);
            #endregion

            #region paging
            int totalProduct = user_Set.Count();
            if (q.start != null && q.limit != null)
            {
                user_Set = user_Set.Skip(q.start.Value).Take(q.limit.Value);
            }
            #endregion

            var results = user_Set.AsEnumerable().Select(item => new
            {
                item.User_ID,
                item.FirstName,
                item.LastName,
                item.FullName,
                item.eMail,
                isCustomerUser = item.Customer_User != null,
                isAdminUser = item.Admin_User != null,
                isAdminUserText = item.Admin_User != null ? "Admin" : ""
            }).ToList();

            return Json(new
            {
                total = totalProduct,
                data = results
            });
        }
        public ActionResult getUser(int id)
        {
            User user = this.user_Repository.FindUser(id);

            if (user == null)
            {
                return Json(new
                {
                    success = false,
                    msg = "User non trouvée"
                });
            }
            var result = new
            {
                user.User_ID,
                user.FirstName,
                user.LastName,
                user.eMail,
                user.FullName,
                isCustomerUser = user.Customer_User != null,
                isAdminUser = user.Admin_User != null,
                isAdminCheckbox = user.Admin_User != null,
                isCurrentUser = user.User_ID == CurrentUser.User.User_ID,
                Password = "",
                ConfirmedPassword = ""
            };

            return Json(new
            {
                success = true,
                data = result
            });
        }

        [FormHandler]
        public ActionResult createUser(Customer_UserForm userForm, bool isAdminCheckbox)
        {
            if (user_Repository.FindCustomer_UserByEmail(userForm.eMail) != null)
            {
                return Json(new
                {
                    success = false,
                    msg = "Un utilisateur avec cet email existe déjà"
                });
            }


            if (String.IsNullOrEmpty(userForm.eMail) || String.IsNullOrEmpty(userForm.FirstName) ||
                String.IsNullOrEmpty(userForm.LastName) || String.IsNullOrEmpty(userForm.Password) ||
                String.IsNullOrEmpty(userForm.ConfirmedPassword) || userForm.ConfirmedPassword.Length < 6 ||
                userForm.Password.Length < 6)
            {
                return Json(new
                {
                    success = false,
                    msg = "Les champs requis ne sont pas remplis"
                });
            }


            if (userForm.Password != userForm.ConfirmedPassword)
            {
                return Json(new
                {
                    success = false,
                    msg = "Les mots de passe ne correspondent pas"
                });
            }
            user_Repository.createCustomer_UserFromForm(userForm, isCreationFromAdmin: true, isUserAdmin: isAdminCheckbox);
            user_Repository.Save();

            return Json(new
            {
                msg = "Utilisateur créé avec succès.",
                success = true
            });
        }

        [FormHandler]
        public ActionResult editInformationUser(Customer_UserForm userForm, bool isAdminCheckbox)
        {
            if (userForm.User_ID == null)
            {
                return Json(new
                {
                    success = false,
                    msg = "Le champ User_ID n'est pas renseigné, l'utilisateur ne peut pas être cherché"
                });
            }
            var customerUser = user_Repository.FindCustomer_User((int)userForm.User_ID);

            if (customerUser == null)
            {
                return Json(new
                {
                    success = false,
                    msg = "L'utilisateur avec User_ID:"+ userForm.User_ID + " ne semble pas exister"
                });
            }

            if (String.IsNullOrEmpty(userForm.eMail) || String.IsNullOrEmpty(userForm.FirstName) ||
                String.IsNullOrEmpty(userForm.LastName))
            {
                return Json(new
                {
                    success = false,
                    msg = "Les champs requis ne sont pas remplis"
                });
            }

            if (!String.IsNullOrEmpty(userForm.Password) || !String.IsNullOrEmpty(userForm.ConfirmedPassword))
            {
                if (userForm.ConfirmedPassword.Length < 6 || userForm.Password.Length < 6)
                {
                    return Json(new
                    {
                        success = false,
                        msg = "Les mots de passent doivent avoir un minimum de 6 caractères"
                    });
                }

                if (userForm.Password != userForm.ConfirmedPassword)
                {
                    return Json(new
                    {
                        success = false,
                        msg = "Les mots de passent ne correspondent pas"
                    });
                }
            }

            //customerUser.User.eMail = userForm.eMail;
            customerUser.User.FirstName = userForm.FirstName;
            customerUser.User.LastName = userForm.LastName;
            customerUser.setSaltHashedSHA256Password(userForm.Password);

            if (customerUser.User.User_ID != CurrentUser.User.User_ID)
            {
                if (isAdminCheckbox)
                {
                    user_Repository.GiveAdminUserRole(customerUser);
                }
                else
                {
                    user_Repository.RemoveAdminUserRole(customerUser);
                }
            }

            user_Repository.Save();

            return Json(new
            {
                msg = "Utilisateur édité avec succès.",
                success = true
            });
        }

    }
}