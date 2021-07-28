using MesJolisCotillons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using MesJolisCotillons.ViewModels;
using MesJolisCotillons.Extensions.Paypal;

namespace MesJolisCotillons.Models
{
    public class Command_Repository : Repository
    {
        private Product_Repository product_Repository = new Product_Repository();
        private User_Repository user_Repository = new User_Repository();
        private Util_Repository util_Repository = new Util_Repository();

        public Command createCommand(MySessionCartModel cart, CheckoutPaymentForm form)
        {

            Customer_User customerUser = null;
            if (CurrentUser.User?.Customer_User != null)
            {
                customerUser = CurrentUser.User.Customer_User;
            }
            else if (!String.IsNullOrEmpty(form.emailNoLogin))
            {
                customerUser = user_Repository.FindCustomer_UserByEmail(form.emailNoLogin, Customer_User_Status.CommandWithoutSubscription);
                if (customerUser == null)
                {
                    customerUser = new Customer_User
                    {
                        User = new User
                        {
                            FirstName = form.FirstNameInvoice,
                            LastName = form.LastNameInvoice,
                            eMail = form.emailNoLogin
                        },
                        Status = Customer_User_Status.CommandWithoutSubscription
                    };
                }
            }

            var command = new Command
            {
                CreationDateTime = DateTime.Now,
                //Customer_User_FK = CurrentUser.User.User_ID,
                Customer_User = customerUser,
                ShipmentType = form.shipmentType
            };

            if (form.shipmentType == ShipmentType.NoShipment)
            {
                command.TotalPrice = cart.TotalCommand;
                command.DeliveryCharge = 0;
            }
            else if (form.shipmentType == ShipmentType.NormalShipment)
            {
                command.TotalPrice = cart.TotalCommandWithDeliveryCharge;
                command.DeliveryCharge = cart.DeliveryCharge;
            }
            else if (form.shipmentType == ShipmentType.OptionSecureShipment)
            {
                command.OptionShipmentSecureType = cart.OptionShipmentSecureType;
                command.OptionShipmentCharge = cart.OptionShipmentPrice;
                command.TotalPrice = cart.TotalCommandWithDeliveryCharge + cart.OptionShipmentPrice;
                command.DeliveryCharge = cart.DeliveryCharge + cart.OptionShipmentPrice;
            }



            #region Command Products
            foreach (var item in cart.commandProducts)
            {
                var product = product_Repository.FindProduct(item.product.Product_ID);
                if (product == null)
                {
                    continue;
                }

                //var commandProducXmlData = new CommandProduct_XmlData
                //{
                //    StockQuantity = (int)product.StockQuantity
                //};

                var commandProduct = new CommandProduct
                {
                    Product_FK = product.Product_ID,
                    Quantity = item.quantity,
                    TotalPrice = item.TotalCommandProduct,
                    ProductPrice = product.Price == null ? 0 : (decimal)product.Price
                    //xmlData = commandProducXmlData.Serialize()
                };

                command.CommandProduct_Set.Add(commandProduct);
            }
            #endregion

            #region Command Addresses


            var countryInvoice = util_Repository.FindCountryByName(form.CountryInvoice);

            var addressInvoice = new CommandAddress
            {
                FirstName = form.FirstNameInvoice,
                LastName = form.LastNameInvoice,
                Address = new Address
                {
                    Address1 = form.AddressInvoice,
                    City = form.CityInvoice,
                    Zip_Code = form.ZipCodeInvoice,
                    Country = countryInvoice
                },
                Type = CommanAddress_Type.Invoice
            };
            command.CommandAddress_Set.Add(addressInvoice);

            if (form.shipmentType != ShipmentType.NoShipment)
            {
                if (!form.isAddressInvoiceSameFromShipment)
                {
                    var countryShipment = util_Repository.FindCountryByName(form.CountryInvoice);
                    var addressShipment = new CommandAddress
                    {
                        FirstName = form.FirstNameShipment,
                        LastName = form.LastNameShipment,
                        Address = new Address
                        {
                            Address1 = form.AddressShipment,
                            City = form.CityShipment,
                            Zip_Code = form.ZipCodeShipment,
                            Country = countryShipment
                        },
                        Type = CommanAddress_Type.Shipment
                    };
                    command.CommandAddress_Set.Add(addressShipment);
                }
                else
                {
                    addressInvoice.Type |= CommanAddress_Type.Shipment;
                }
            }

            #endregion

            #region History and Status
            command.addCommand_History(Command_History_Action.Command_Created, "Commande créé");
            command.Command_Status = Command_Status.Command_AwaitingPayment;
            command.addCommand_History(Command_History_Action.Command_AwaitingPayment, "Commande passée au statut: En attente de paiement");
            #endregion

            db.Commands.Add(command);

            return command;
        }

        public IQueryable<Command> FindAllCommand()
        {
            return db.Commands;
        }

        public IQueryable<CommandProduct> FindAllCommandProduct()
        {
            return db.CommandProducts;
        }

        public IQueryable<Command_Document> FindAllCommand_Document()
        {
            return db.Command_Document;
        }

        public IQueryable<Command_History> FindAllCommand_History()
        {
            return db.Command_History;
        }


        public Command FindCommand(int id)
        {
            var command = db.Commands.Where(item => item.Command_ID == id)
                                     .FirstOrDefault();

            return command;
        }

        public void deleteCommandDocument(Command_Document commandDoc)
        {
            var blob = commandDoc.Document.Blob;
            var document = commandDoc.Document;

            this.db.Command_Document.Remove(commandDoc);
            this.db.Documents.Remove(document);
            this.db.Blobs.Remove(blob);
        }
    }
}