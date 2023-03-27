using System.Xml;

namespace FlowGraph
{
    public static class XmlExtension
    {
        public static XmlElement AddRootNode(this XmlDocument xmlDoc, string nodeName)
        {
            //let's add the XML declaration section
            var xmlnode = xmlDoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
            xmlDoc.AppendChild(xmlnode);

            //let's add the root element
            var xmlElem = xmlDoc.CreateElement("", nodeName, "");
            xmlDoc.AppendChild(xmlElem);

            return xmlElem;
        }

        public static void AddAttribute(this XmlNode xmlNode, string attributeName, string? value)
        {
            if (xmlNode.OwnerDocument != null)
            {
                var att = xmlNode.OwnerDocument.CreateAttribute(attributeName);
                att.Value = value;
                xmlNode.Attributes.Append(att);
            }
        }

        public static XmlElement CreateElementWithText(this XmlDocument xmlDoc, string nodeName, string val)
        {
            var el = xmlDoc.CreateElement(nodeName);
            var txtXml = xmlDoc.CreateTextNode(val);
            el.AppendChild(txtXml);

            return el;
        }

        public static object LoadValue(this XmlNode node)
        {
            if (node.Attributes != null)
            {
                var typeValue = node.Attributes["valueType"].Value;

                if (string.Equals(typeValue, "null"))
                {
                    return null;
                }

                var type = Type.GetType(typeValue);
                return Convert.ChangeType(node.InnerText, type);
            }

            return null;
        }

        public static void SaveValue(this XmlNode node, object value)
        {
            if (value != null)
            {
                var typeName = value.GetType().AssemblyQualifiedName;
                var index = typeName.IndexOf(',', typeName.IndexOf(',') + 1);
                typeName = typeName.Substring(0, index);
                node.AddAttribute("valueType", typeName);
                node.InnerText = Convert.ToString(value);
            }
            else
            {
                node.AddAttribute("valueType", "null");
            }
        }
    }
}
