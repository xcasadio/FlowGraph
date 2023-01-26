using System.Xml;

namespace FlowSimulator
{
    public static class XmlExtension
    {
        public static XmlElement AddRootNode(this XmlDocument xmlDoc, string nodeName)
        {
            XmlNode xmlnode = xmlDoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
            xmlDoc.AppendChild(xmlnode);

            XmlElement xmlElem = xmlDoc.CreateElement("", nodeName, "");
            xmlDoc.AppendChild(xmlElem);

            return xmlElem;
        }

        public static void AddAttribute(this XmlNode node, string attributeName, string value)
        {
            if (node.OwnerDocument != null)
            {
                XmlAttribute att = node.OwnerDocument.CreateAttribute(attributeName);
                att.Value = value;
                node.Attributes?.Append(att);
            }
        }

        public static void AddElementWithText(this XmlNode node, string nodeName, string val)
        {
            if (node.OwnerDocument != null)
            {
                XmlElement el = node.OwnerDocument.CreateElement(nodeName);
                XmlText txtXml = node.OwnerDocument.CreateTextNode(val);
                el.AppendChild(txtXml);
            }
        }
    }
}
