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
    public class DocumentDirectController : DirectController
    {
        Document_Repository document_Repository = new Document_Repository();


        public ActionResult getDocumentDetails(int id)
        {
            var item = document_Repository.FindDocument(id);

            var result = new
            {
                Document_ID = item.Document_ID,
                DateUploaded = item.Blob.CreationDateTime == null ? "" : ((DateTime)item.Blob.CreationDateTime).ToString("dd-MM-yyyy - HH:mm"),
                DocumentFileName = item.Blob.FileName,
                FileType = item.FileType.ToString()
            };

            return Json(new
            {
                data = result,
                success = true
            });

        }

        [DirectIgnore]
        [OutputCacheAttribute(VaryByParam = "*", Duration = 60)]
        public FileContentResult getDocument(int id)
        {
            FileContentResult file = null;
            DateTime? date = null;

            var doc = document_Repository.FindDocument(id);

            file = new FileContentResult(doc.Blob.Stream, doc.Blob.MimeType);
            date = doc.Blob.CreationDateTime == null ? DateTime.Now : ((DateTime)doc.Blob.CreationDateTime).ToUniversalTime();

            Response.AppendHeader("Last-Modified", String.Format("{0:r}", date));

            return file;
        }

        [DirectIgnore]
        [OutputCacheAttribute(VaryByParam = "*", Duration = 60)]
        public FileContentResult downloadDocument(int id)
        {
            FileContentResult file = null;
            DateTime? date = null;

            var doc = document_Repository.FindDocument(id);

            file = new FileContentResult(doc.Blob.Stream, doc.Blob.MimeType);
            date = ((DateTime)doc.Blob.CreationDateTime).ToUniversalTime();

            Response.AppendHeader("Last-Modified", String.Format("{0:r}", date));
            file.FileDownloadName = doc.Blob.FileName;

            return file;
        }
    }
}