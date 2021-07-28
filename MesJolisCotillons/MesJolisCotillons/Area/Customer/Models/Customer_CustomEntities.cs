using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MesJolisCotillons.Area.Customer;
using MesJolisCotillons.Extensions.Security;
using MesJolisCotillons.Extensions.XmlData;


namespace MesJolisCotillons.Models
{

    public partial class Customer_User
    {
        private Customer_XmlData _customerXmlConfig;

        public string InscriptionTokenLink
        {
            get
            {
                string tokenLink = null;
                if (this.Customer_XmlData != null)
                {

                    tokenLink = this.Customer_XmlData.InsriptionConfirmationProcess?.TokenLinkGenerated;
                }

                return tokenLink;
            }
        }

        public bool IsIncriptionLinkConfirmed
        {
            get
            {
                return this?.Customer_XmlData?.InsriptionConfirmationProcess?.Status == InscriptionConfirmationStatus.emailConfirmed;
            }
        }

        public bool isRedefiningPasswordTokenStillValid(string tokenLink)
        {
            return this.Customer_XmlData.RedefinitionPasswordProcess_Set.Any(item => item.TokenLinkGenerated == tokenLink && item.Status == RedefinePasswordProcessStatus.emailSent);
        }
        public Customer_XmlData Customer_XmlData
        {
            get
            {
                if (_customerXmlConfig == null)
                {
                    _customerXmlConfig = XmlSerializer<Customer_XmlData>.Deserialize<Customer_XmlData>(this.xmlData);
                }

                return _customerXmlConfig;
            }
        }

        public string generateInscriptionTokenLink()
        {
            var saltItem = CryptographyUtils.GenerateSHA256SaltedText(this.User.eMail);

            this.xmlData = new Customer_XmlData()
            {
                InsriptionConfirmationProcess = new InsriptionConfirmationProcess()
                {
                    Salt = saltItem.Salt,
                    SaltAndEmail = saltItem.Salt + saltItem.Text,
                    TokenLinkGenerated = saltItem.HashedText,
                    InscriptionActionHistory = new List<InscriptionActionHistory>()
                        {
                            new InscriptionActionHistory()
                            {
                                Date = DateTime.Now,
                                Status = InscriptionConfirmationStatus.emailSent
                            }
                        }
                }
            }.Serialize();

            return saltItem.HashedText;
        }
        public string generatePasswordRedefinitionTokenLink()
        {
            var saltItem = CryptographyUtils.GenerateSHA256SaltedText(this.User.eMail);

            var redinitionPasswordProcess = new RedefinitionPasswordProcess
            {
                Salt = saltItem.Salt,
                SaltAndEmail = saltItem.Salt + saltItem.Text,
                TokenLinkGenerated = saltItem.HashedText,
                RedefinePasswordProcessActionHistory = new List<RedefinePasswordProcessActionHistory>()
                        {
                            new RedefinePasswordProcessActionHistory()
                            {
                                Date = DateTime.Now,
                                Status = RedefinePasswordProcessStatus.emailSent
                            }
                        }
            };

            this.Customer_XmlData.RedefinitionPasswordProcess_Set.Add(redinitionPasswordProcess);
            this.xmlData = this.Customer_XmlData.Serialize();

            return saltItem.HashedText;
        }

        public void setSaltHashedSHA256Password(string password)
        {
            var saltItem = CryptographyUtils.GenerateSHA256SaltedText(password);

            this.User.Salt = saltItem.Salt;
            this.User.Password = saltItem.HashedText;
        }
        public void SetInscriptionUserConfirmed()
        {
            if (this.Customer_XmlData == null)
                return;

            this.Customer_XmlData.InsriptionConfirmationProcess.Status = InscriptionConfirmationStatus.emailConfirmed;
            this.Customer_XmlData.InsriptionConfirmationProcess.InscriptionActionHistory.Add(new InscriptionActionHistory()
            {
                Date = DateTime.Now,
                Status = InscriptionConfirmationStatus.emailConfirmed
            });
            this.xmlData = this.Customer_XmlData.Serialize();
            this.Status = Customer_User_Status.Active;
        }
        public bool setRedefinedPasswordProcessStatus(RedefinePasswordProcessStatus status, string tokenLink)
        {
            var result = true;

            var redefinePasswordProcess = this.Customer_XmlData.RedefinitionPasswordProcess_Set.Where(item => item.TokenLinkGenerated == tokenLink).FirstOrDefault();

            if (redefinePasswordProcess == null)
            {
                result = false;
            }

            redefinePasswordProcess.Status = status;
            this.xmlData = this.Customer_XmlData.Serialize();

            return result;
        }

        public void setCustomerPhone(string phone)
        {
            this.Customer_XmlData.Phone = phone;
            this.xmlData = this.Customer_XmlData.Serialize();
        }

        public CustomerAddress CustomerAddressShipment
        {
            get
            {
                var result = this.CustomerAddress_Set.Where(a => a.Type != null && ((CustomerAddress_Type)a.Type).HasFlag(CustomerAddress_Type.Shipment))
                                                  .FirstOrDefault();

                return result;
            }
        }

        public CustomerAddress CustomerAddressInvoice
        {
            get
            {
                var result = this.CustomerAddress_Set.Where(a => a.Type != null && ((CustomerAddress_Type)a.Type).HasFlag(CustomerAddress_Type.Invoice))
                                                  .FirstOrDefault();

                return result;
            }
        }

        public bool HasBeenAlreadySuscribed
        {
            get
            {
                return this.Status >= Customer_User_Status.Creation || this.Status == Customer_User_Status.Disabled;
            }
        }
    }

    //public enum Customer_User_Status
    //{
    //    Disabled = -200,
    //    Creation = 0,
    //    Awaiting_MailConfirmation = 100,
    //    Active = 200,
    //}
}


namespace MesJolisCotillons.Area.Customer
{
    public class InscriptionActionHistory
    {
        public DateTime Date { get; set; }

        public InscriptionConfirmationStatus Status { get; set; }
    }
    public class RedefinePasswordProcessActionHistory
    {
        public DateTime Date { get; set; }

        public RedefinePasswordProcessStatus Status { get; set; }
    }
    public enum InscriptionConfirmationStatus
    {
        emailSent = 0,
        tokenExpired = 100,
        emailConfirmed = 200
    }
    public enum RedefinePasswordProcessStatus
    {
        emailSent = 0,
        tokenExpired = 100,
        passwordRedefined = 200
    }
    public class TokenLinkGeneratedProcess
    {
        public string TokenLinkGenerated { get; set; }
        public string Salt { get; set; }
        public string SaltAndEmail { get; set; }
    }
    public class InsriptionConfirmationProcess : TokenLinkGeneratedProcess
    {
        public InscriptionConfirmationStatus Status { get; set; }
        public List<InscriptionActionHistory> InscriptionActionHistory { get; set; }
    }
    public class RedefinitionPasswordProcess : TokenLinkGeneratedProcess
    {
        public RedefinePasswordProcessStatus Status { get; set; }
        public List<RedefinePasswordProcessActionHistory> RedefinePasswordProcessActionHistory { get; set; }
    }
}