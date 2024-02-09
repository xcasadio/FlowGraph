using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CSharpSyntax.Printer.Configuration
{
    [XmlRoot(ElementName = "Configuration", Namespace = Ns)]
    public class SyntaxPrinterConfiguration
    {
        public const string Ns = "https://github.com/pvginkel/CSharpSyntax/SyntaxPrinterConfiguration";

        [DefaultValue(4)]
        public int Indentation { get; set; }

        [DefaultValue(IndentationStyle.Spaces)]
        public IndentationStyle IndentationStyle { get; set; }

        public BracesLayoutConfiguration BracesLayout { get; set; }

        public BlankLinesConfiguration BlankLines { get; set; }

        public LineBreaksAndWrappingConfiguration LineBreaksAndWrapping { get; set; }

        public SpacesConfiguration Spaces { get; set; }

        public OtherConfiguration Other { get; set; }

        public SyntaxPrinterConfiguration()
        {
            XmlSerializationUtil.AssignDefaultValues(this);

            BracesLayout = new BracesLayoutConfiguration();
            BlankLines = new BlankLinesConfiguration();
            LineBreaksAndWrapping = new LineBreaksAndWrappingConfiguration();
            Spaces = new SpacesConfiguration();
            Other = new OtherConfiguration();
        }

        public static SyntaxPrinterConfiguration Load(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            using (var stream = File.OpenRead(fileName))
            {
                return Load(stream);
            }
        }

        public static SyntaxPrinterConfiguration Load(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            using (var reader = new StreamReader(stream))
            {
                return Load(reader);
            }
        }

        public static SyntaxPrinterConfiguration Load(TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            return Serialization.DeserializeXml<SyntaxPrinterConfiguration>(reader.ReadToEnd());
        }

        public void Save(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            using (var stream = File.Create(fileName))
            {
                Save(stream);
            }
        }

        public void Save(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            using (var writer = new StreamWriter(stream))
            {
                Save(writer);
            }
        }

        public void Save(TextWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            writer.Write(ToString());
        }

        public override string ToString()
        {
            var doc = new XmlDocument();

            doc.LoadXml(Serialization.SerializeXml(this));

            using (var writer = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  ",
                    OmitXmlDeclaration = true
                }))
                {
                    doc.Save(xmlWriter);
                }

                return writer.GetStringBuilder().ToString();
            }
        }
    }
}
