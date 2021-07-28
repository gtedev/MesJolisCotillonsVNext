using MesJolisCotillons.Extensions.XmlData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace MesJolisCotillons.Extensions.XmlData
{
    public abstract class XmlSerializer<T>
    {

        public string Serialize()
        {
            string xmlData;
            using (StringWriter writer = new StringWriter(CultureInfo.InvariantCulture))
            {
                XmlSerializer serializer = new XmlSerializer(this.GetType());
                serializer.Serialize(writer, this);
                xmlData = writer.ToString();
            }
            return xmlData;
        }

        public static string Serialize(object obj)
        {
            string xmlData;
            using (StringWriter writer = new StringWriter(CultureInfo.InvariantCulture))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(writer, obj);
                xmlData = writer.ToString();
            }
            return xmlData;
        }

        public static L Deserialize<L>(string xmlData)
        {
            if (String.IsNullOrWhiteSpace(xmlData))
                return (L)Activator.CreateInstance(typeof(L));

            L result;
            using (XmlReader xmlReader = XmlReader.Create(new StringReader(xmlData)))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(L));
                result = (L)deserializer.Deserialize(xmlReader);
            }
            return result;
        }
    }
}