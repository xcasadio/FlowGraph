using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CSharpSyntax
{
    internal static class Serialization
    {
        public static string SerializeXml<T>(T obj)
        {
            return SerializeXml<T>(obj, null);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static string SerializeXml<T>(T obj, string defaultNamespace)
        {
            var settings = new XmlWriterSettings();

            settings.OmitXmlDeclaration = true;
            settings.CloseOutput = false;

            using (var writer = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(writer, settings))
                {
                    GetSerializer<T>(defaultNamespace).Serialize(xmlWriter, obj);
                }

                return writer.ToString();
            }
        }

        private static XmlSerializer GetSerializer<T>(string defaultNamespace)
        {
            if (String.IsNullOrEmpty(defaultNamespace))
            {
                return new XmlSerializer(typeof(T));
            }
            else
            {
                return new XmlSerializer(typeof(T), defaultNamespace);
            }
        }

        public static T DeserializeXml<T>(string xml)
        {
            return DeserializeXml<T>(xml, null);
        }

        public static T DeserializeXml<T>(string xml, string defaultNamespace)
        {
            using (var reader = new StringReader(xml))
            {
                return (T)GetSerializer<T>(defaultNamespace).Deserialize(reader);
            }
        }
    }
}
