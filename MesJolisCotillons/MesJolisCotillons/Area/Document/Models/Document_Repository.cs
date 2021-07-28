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
    public class Document_Repository : Repository
    {
        public IQueryable<Document> FindAllDocument()
        {
            return db.Documents;
        } 

        public Document FindDocument(int id)
        {
            var document = db.Documents.Where(item => item.Document_ID == id)
                                     .FirstOrDefault();

            return document;
        } 
    }
}