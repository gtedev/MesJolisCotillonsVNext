using Ext.Direct.Mvc;
using MesJolisCotillons.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MesJolisCotillons.Area._Admin.Controllers.Direct
{
    [AdminAuthorize]
    [DirectHandleError]
    public class CommandDirectController : DirectController
    {
        Command_Repository command_Repository = new Command_Repository();
        Document_Repository document_Repository = new Document_Repository();

        public ActionResult getCommandTree_Set()
        {

            var commandByYears = command_Repository.FindAllCommand().GroupBy(item => item.CreationDateTime.Value.Year);

            var results = commandByYears.AsEnumerable().Select(item =>
                            new
                            {
                                text = item.Key,
                                leaf = item.Count() == 0,
                                children = item.GroupBy(c => c.CreationDateTime.Value.Month).Select(d => new
                                {
                                    text = d.Key.ToFrenchMonthName(),
                                    leaf = true
                                })
                                //expanded = false
                            }).ToList();

            return Json(new
            {
                children = results
            });

        }

        public ActionResult getCommand_Set(DirectStoreQuery q)
        {
            var command_Set = command_Repository.FindAllCommand();

            #region filters
            if (q.filterHasProperty("Status"))
            {
                int status = Int32.Parse(q.filterValue("Status"));
                if (status != -1000) //-1000 is filter ALL
                {
                    command_Set = command_Set.Where(item => item.Command_Status == (Command_Status)status);
                }
            }
            #endregion

            #region orderby
            command_Set = command_Set.OrderByDescending(item => item.Command_ID);
            #endregion

            #region paging
            int totalProduct = command_Set.Count();
            if (q.start != null && q.limit != null)
            {
                command_Set = command_Set.Skip(q.start.Value).Take(q.limit.Value);
            }
            #endregion

            var results = command_Set.AsEnumerable().Select(item => new
            {
                item.Command_ID,
                CustomerFullName = item.Customer_User.User.FullName,
                CreationDateTime = item.CreationDateTime?.ToString("dd-MM-yyyy"),
                item.TotalPrice,
                StatusInt = item.Command_Status,
                Status = item.Command_Status.ToStringFrench(),
                Destination = item.CommandAddressShipment != null ? item.CommandAddressShipment.Address.City + ", " + item.CommandAddressShipment.Address.Country.Name : " - "
            }).ToList();

            return Json(new
            {
                total = totalProduct,
                data = results
            });
        }

        public ActionResult getCommandProduct_Set(DirectStoreQuery q)
        {
            var command_Set = command_Repository.FindAllCommand();
            IQueryable<CommandProduct> commandProduct_Set = command_Repository.FindAllCommandProduct();

            #region filters
            if (q.filterHasProperty("Command_ID"))
            {
                int commandId = Int32.Parse(q.filterValue("Command_ID"));
                if (commandId != -1000) //-1000 is filter ALL
                {
                    var command = command_Set.Where(item => item.Command_ID == commandId).FirstOrDefault();
                    commandProduct_Set = command.CommandProduct_Set.AsQueryable();
                }
            }
            #endregion

            #region orderby
            commandProduct_Set = commandProduct_Set.OrderBy(item => item.CommandProduct_ID);
            #endregion

            #region paging
            int totalCommandProduct = commandProduct_Set.Count();
            if (q.start != null && q.limit != null)
            {
                commandProduct_Set = commandProduct_Set.Skip(q.start.Value).Take(q.limit.Value);
            }
            #endregion

            var results = commandProduct_Set.Select(item => new
            {
                Product_ID = item.Product_FK,
                item.Product.DisplayName,
                item.Product.Name,
                item.Product.Price,
                item.Quantity,
                item.TotalCommandProduct
            }).ToList();

            return Json(new
            {
                total = totalCommandProduct,
                data = results
            });
        }

        public ActionResult getInformationCommand(int id)
        {
            var command = command_Repository.FindCommand(id);

            //Command_History_XmlData commandInformationDelivered = null;
            //if (command.Command_Status == Command_Status.Command_Delivered)
            //{
            //    commandInformationDelivered = command.Command_History_Set.Where(item => item.Command_History_Action == Command_History_Action.)
            //}
            var result = new
            {
                command.Command_ID,
                command.DeliveryCharge,
                CreationDateTime = command?.CreationDateTime?.ToString("dd-MM-yyyy"),
                Status = command.Command_Status.ToStringFrench(),
                StatusInt = command.Command_Status,
                CustomerFullName = command.Customer_User.User.FullName,
                command.Customer_User_FK,
                eMail = command.Customer_User.User.eMail,
                OptionShipmentSecureType = command.OptionShipmentSecureType?.ToString(),
                OptionShipmentCharge = command.OptionShipmentCharge,
                isOptionShipmentChosen = command.OptionShipmentSecureType != null,
                ShipmentType = command.ShipmentType,
                ShipmentTypeString = command.ShipmentType != null ? ((ShipmentType)command.ShipmentType).ToStringFrench() : null,
                command.TotalPrice,
                Address = new
                {
                    CustomerFullName = command.CommandAddressShipment?.FullName,
                    Address1 = command.CommandAddressShipment?.Address.Address1,
                    City = command.CommandAddressShipment?.Address.City,
                    ZipCode = command.CommandAddressShipment?.Address.Zip_Code,
                    Country = command.CommandAddressShipment?.Address.Country.Name
                },
                CommandInformationDelivered = new
                {
                    CommandChoiceAction = command?.Command_XmlData?.CommandChoiceAction?.ToStringFrench(),
                    CommandChoiceActionInt = command?.Command_XmlData?.CommandChoiceAction,
                    ColissimoNumber = command?.Command_XmlData?.ColissimoNumber,
                    ShipmentDate = command?.Command_XmlData.ShipmentDate != null ? ((DateTime)command?.Command_XmlData.ShipmentDate).ToString("dd-MM-yyyy") : null,
                    FeesPaidInPostOffice = command?.Command_XmlData?.FeesPaidInPostOffice + " €",
                    Paypal_Txn_id = command?.Command_XmlData?.PaypalNotificationForm?.txn_id,
                    CommentFromAdminUser = command?.Command_XmlData?.CommentFromAdminUser,
                }
            };

            return Json(new
            {
                success = true,
                data = result
            });
        }

        [FormHandler]
        public ActionResult actionOnCommand(CommandActionForm form)
        {

            var command = command_Repository.FindCommand(form.Command_ID);
            if (command == null)
            {
                throw new Exception("La commande avec ID:" + form.Command_ID + "ne semble plus exister");
            }

            switch (form.CommandChoiceAction)
            {
                case Command_History_Action.Command_Delivered:
                    command.Command_Status = Command_Status.Command_Delivered;
                    command.addCommand_History(form.CommandChoiceAction, "Commande postée. Montant payée: " + form.FeesPaidInPostOfficeField, form.CommentFromAdminUser, form.ShipmenDate, form.FeesPaidInPostOfficeField, ColissimoNumber: form.ColissimoNumber);
                    break;
                case Command_History_Action.CommandForceToStatusPaid:
                    command.Command_Status = Command_Status.Command_Paid;
                    command.addCommand_History(form.CommandChoiceAction, "Commande passée manuellement au statut: \"Payée\". Le client a payé par " + ((CommandActionRadioForcePayment)form.radioButtonForcePayment).ToStringFrench(), form.CommentFromAdminUser, ForcePaymentReason: form.radioButtonForcePayment);
                    break;
                case Command_History_Action.CommandDeliveredPersonally:
                    command.Command_Status = Command_Status.Command_Delivered;
                    command.addCommand_History(form.CommandChoiceAction, "Commande Delivrée en mains propres", form.CommentFromAdminUser, form.ShipmenDate);
                    break;
                case Command_History_Action.Command_Disabled:
                    command.Command_Status = Command_Status.Command_Disabled;
                    command.addCommand_History(form.CommandChoiceAction, "Commande Désactivée", form.CommentFromAdminUser);
                    break;
            }

            command.setActionChoiceCommandCommandXmlData(form);
            command_Repository.Save();

            return Json(new
            {
                success = true
            });
        }

        public ActionResult getCommand_History_Set(DirectStoreQuery q)
        {

            var commandHistory_Set = command_Repository.FindAllCommand_History();

            #region filters
            if (q.filterHasProperty("Command_ID"))
            {
                int commandId = Int32.Parse(q.filterValue("Command_ID"));
                commandHistory_Set = commandHistory_Set.Where(item => item.Command_FK == commandId);
            }
            #endregion

            #region orderby
            commandHistory_Set = commandHistory_Set.OrderByDescending(item => item.Command_History_ID);
            #endregion

            #region paging
            int totalProduct = commandHistory_Set.Count();
            if (q.start != null && q.limit != null)
            {
                commandHistory_Set = commandHistory_Set.Skip(q.start.Value).Take(q.limit.Value);
            }
            #endregion

            var results = commandHistory_Set.AsEnumerable().Select(item => new
            {
                item.Command_FK,
                Date = item.Date.ToString("dd-MM-yyyy - HH:mm"),
                Description = item.Command_History_XmlData.Description,
                CommentFromAdminUser = item.Command_History_XmlData.CommentFromAdminUser
            }).ToList();

            return Json(new
            {
                total = totalProduct,
                data = results
            });
        }

        public ActionResult getCommandDocument_Set(DirectStoreQuery q)
        {
            var command_Set = command_Repository.FindAllCommand();
            IQueryable<Command_Document> commandDocument_Set = command_Repository.FindAllCommand_Document();

            #region filters
            if (q.filterHasProperty("Command_ID"))
            {
                int commandId = Int32.Parse(q.filterValue("Command_ID"));

                var command = command_Set.Where(item => item.Command_ID == commandId).FirstOrDefault();
                commandDocument_Set = command.Command_Document_Set.AsQueryable();
            }
            #endregion

            #region orderby
            commandDocument_Set = commandDocument_Set.OrderBy(item => item.Document_FK);
            #endregion

            #region paging
            int totalCommandDocument = commandDocument_Set.Count();
            if (q.start != null && q.limit != null)
            {
                commandDocument_Set = commandDocument_Set.Skip(q.start.Value).Take(q.limit.Value);
            }
            #endregion

            var results = commandDocument_Set.Select(item => new
            {
                Document_ID = item.Document.Document_ID,
                item.Document.Name,
                item.Document.Description,
                CreationDateTime = item.Document.Blob.CreationDateTime != null ? ((DateTime)item.Document.Blob.CreationDateTime).ToString("dd-MM-yyyy") : "",
                FileType = item.Document.FileType.ToString(),
            }).ToList();

            return Json(new
            {
                total = totalCommandDocument,
                data = results
            });
        }

        public ActionResult deleteCommandDocument(int Document_FK)
        {
            var doc = document_Repository.FindDocument(Document_FK);

            if (doc == null)
            {
                throw new Exception("Document avec le Document_ID:" + Document_FK + "ne semble plus exister");
            }

            var commandDocument = doc.Command_Document;
            if (commandDocument == null)
            {
                throw new Exception("CommandDocument avec le Document_FK:" + Document_FK + "ne semble plus exister");
            }
            command_Repository.deleteCommandDocument(commandDocument);

            command_Repository.Save();

            return Json(new
            {
                success = true,
                msg = "Document deleted Successfully"
            });
        }

        [FormHandler]
        public ActionResult UploadCommandDocument(string Description, int Command_ID)
        {
            HttpPostedFileBase hpf;

            var command = command_Repository.FindCommand(Command_ID);
            if (command == null)
            {
                throw new Exception("La commande avec ID:" + Command_ID + "ne semble plus exister");
            }

            #region Ontology
            var files = Request.Files;
            if (Request.Files.Count < 1)
                throw new Exception("Aucun fichier n'a été soumis !");
            if (Request.Files.Count > 1)
                throw new Exception("Plus de 1 ficheir a été soumis !");

            hpf = Request.Files[0];
            if (hpf.ContentLength == 0)
                throw new Exception("Le fichier est-il vide ? ?");

            if (hpf.ContentLength > 10000000)
            {
                return Json(new
                {
                    msg = "Le fichier est trop large, il doit être en dessous de 10MB",
                    success = false
                });
            }

            #endregion

            #region CommandDocument Creation

            byte[] binaryData = new byte[hpf.ContentLength];
            hpf.InputStream.Read(binaryData, 0, hpf.ContentLength);

            var fileBLob = new Blob
            {
                CreationDateTime = DateTime.Now,
                FileName = hpf.FileName,
                MimeType = hpf.ContentType,
                Stream = binaryData
            };
            var document = new Document
            {
                Blob = fileBLob,
                Name = hpf.FileName,
                Description = Description
            };

            var commandDocument = new Command_Document
            {
                Document = document
            };

            #endregion

            command.Command_Document_Set.Add(commandDocument);
            command_Repository.Save();

            return Json(new
            {
                msg = "Fichier upoloadé avec succès",
                document.Document_ID,
                success = true
            });
        }

        #region Useful class
        public class CommandActionForm
        {
            public int Command_ID { get; set; }
            public Command_History_Action CommandChoiceAction { get; set; }
            public string CommentFromAdminUser { get; set; }
            public CommandActionRadioForcePayment? radioButtonForcePayment { get; set; }
            public decimal? FeesPaidInPostOfficeField { get; set; }
            public DateTime? ShipmenDate { get; set; }
            public string ColissimoNumber { get; set; }
        }

        public enum CommandActionRadioForcePayment : int
        {
            Cheque = 10,
            TransferPayment = 20,
            Cash = 30,
            PaypalOutOfWebSite = 40,
            Other = 50
        }
        #endregion
    }
}