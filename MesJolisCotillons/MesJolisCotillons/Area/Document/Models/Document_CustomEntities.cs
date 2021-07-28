using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MesJolisCotillons.Models
{
    public partial class Document
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public FileType FileType
        {
            get
            {
                FileType type;

                switch (this.Blob.MimeType)
                {

                    case "application/pdf":
                        type = FileType.PDF;
                        break;
                    case "application/msword":
                    case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
                        type = FileType.WORD;
                        break;
                    case "application/vnd.ms-excel":
                    case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
                        type = FileType.EXCEL;
                        break;
                    case "application/powerpoint":
                    case "application/vnd.ms-powerpoint":
                    case "application/mspowerpoint":
                    case "application/x-mspowerpoint":
                    case "application/vnd.openxmlformats-officedocument.presentationml.presentation":
                        type = FileType.PPT;
                        break;
                    case "image/jpeg":
                    case "image/bmp":
                    case "image/png":
                        type = FileType.IMAGE;
                        break;
                    default:
                        type = FileType.FILE_TYPE_UNKNOWN;
                        break;
                }

                return type;

            }
        }
    }

    public enum FileType
    {
        PDF = 0,
        WORD = 1,
        EXCEL = 2,
        PPT = 3,
        IMAGE = 4,
        FILE_TYPE_UNKNOWN
    }
}