using MesJolisCotillons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Security.Cryptography;
using System.Text;
using MesJolisCotillons.Extensions.Security;
using static MesJolisCotillons.Area.LoginController;

namespace MesJolisCotillons.Models
{
    public class User_Repository : Repository
    {
        public Customer_User authenticateCustomerUser(User userModel)
        {
            Customer_User authenticatedCustomerUser = null;
            var emailTrimed = userModel.eMail?.Trim();
            authenticatedCustomerUser = FindCustomer_UserByEmail(emailTrimed, Customer_User_Status.Active);

            if (authenticatedCustomerUser == null)
                return authenticatedCustomerUser;

            var testSaltPassword = authenticatedCustomerUser.User.Salt + userModel.Password;
            var hashedTestPassword = CryptographyUtils.GenerateSHA256String(testSaltPassword);

            if (authenticatedCustomerUser.User.Password == hashedTestPassword)
            {
                return authenticatedCustomerUser;
            }
            else
            {
                return null;
            }
        }

        public void addUser(User user)
        {
            db.Users.Add(user);
        }

        public IQueryable<User> FindAllUser()
        {
            return db.Users;
        }

        public Customer_User FindCustomer_User(int user_ID)
        {
            return db.Customer_User.Where(item => item.User_FK == user_ID)
                                        .FirstOrDefault();
        }
        public User FindUser(int user_ID)
        {
            return db.Users.Where(item => item.User_ID == user_ID)
                                        .FirstOrDefault();
        }

        public Customer_User createCustomer_UserFromForm(Customer_UserForm customerForm, bool isCreationFromAdmin = false, bool isUserAdmin = false, Customer_User existingUser = null)
        {
            Customer_User customerUser = existingUser;
            if (existingUser == null)
            {
                customerUser = new Customer_User
                {
                    User = new User()
                };
                db.Customer_User.Add(customerUser);
            }

            customerUser.User.eMail = customerForm.eMail;
            customerUser.User.FirstName = customerForm.FirstName;
            customerUser.User.LastName = customerForm.LastName;

            customerUser.setSaltHashedSHA256Password(customerForm.Password);

            if (!isCreationFromAdmin)
            {
                customerUser.Status = Customer_User_Status.Awaiting_MailConfirmation;
            }
            else
            {
                customerUser.Status = Customer_User_Status.Active;
            }

            if (isUserAdmin)
            {
                customerUser.User.Admin_User = new Admin_User();
            }

            #region Customer Address
            Address addressShipment = new Address();
            CustomerAddress customerAddressShipment = new CustomerAddress
            {
                Address = addressShipment,
                Type = CustomerAddress_Type.Shipment
            };
            Address addressInvoice = new Address();
            CustomerAddress customerAddressInvoice = new CustomerAddress
            {
                Address = addressInvoice,
                Type = CustomerAddress_Type.Invoice
            };

            if (!String.IsNullOrEmpty(customerForm.Address))
            {
                addressInvoice.Address1 = customerForm.Address;
                addressShipment.Address1 = customerForm.Address;
            }

            if (!String.IsNullOrEmpty(customerForm.City))
            {
                addressInvoice.City = customerForm.City;
                addressShipment.City = customerForm.City;
            }

            if (!String.IsNullOrEmpty(customerForm.ZipCode))
            {
                addressInvoice.Zip_Code = customerForm.ZipCode;
                addressShipment.Zip_Code = customerForm.ZipCode;
            }

            int countryId;
            if (!String.IsNullOrEmpty(customerForm.Country) && Int32.TryParse(customerForm.Country, out countryId))
            {

                bool isParseOk = Int32.TryParse(customerForm.Country, out countryId);
                var country = new Util_Repository().FindCountry(countryId);
                if (country != null)
                {
                    addressInvoice.Country = country;
                    addressShipment.Country = country;
                }
            }
            customerUser.CustomerAddress_Set.Add(customerAddressShipment);
            customerUser.CustomerAddress_Set.Add(customerAddressInvoice);

            #endregion


            return customerUser;
        }

        public IQueryable<Customer_User> FindAllCustomer_User()
        {
            return db.Customer_User;
        }

        public Customer_User FindCustomer_UserByEmail(string eMail, Customer_User_Status? status = null)
        {
            if (status != null)
            {
                return this.FindAllCustomer_User().Where(item => item.User.eMail == eMail && item.Status == status)
                                                       .FirstOrDefault();
            }
            else
            {
                return this.FindAllCustomer_User().Where(item => item.User.eMail == eMail)
                                  .FirstOrDefault();
            }
        }

        public void GiveAdminUserRole(Customer_User customerUser)
        {
            if (customerUser.User.Admin_User == null)
            {
                customerUser.User.Admin_User = new Admin_User();
            }
        }

        public void RemoveAdminUserRole(Customer_User customerUser)
        {
            if (customerUser.User.Admin_User != null)
            {
                var admin_User = customerUser.User.Admin_User;
                db.Admin_User.Remove(admin_User);
            }
        }
    }
}