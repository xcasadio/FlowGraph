using System.Xml;

namespace FlowSimulator
{
    /// <summary>
    /// 
    /// </summary>
    public static class XmlExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="nodeName"></param>
        /// <returns>the root node added</returns>
        public static XmlElement AddRootNode(this XmlDocument xmlDoc, string nodeName)
        {
            //let's add the XML declaration section
            XmlNode xmlnode = xmlDoc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
            xmlDoc.AppendChild(xmlnode);

            //let's add the root element
            XmlElement xmlElem = xmlDoc.CreateElement("", nodeName, "");
            xmlDoc.AppendChild(xmlElem);

            return xmlElem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attributeName"></param>
        /// <param name="value"></param>
        public static void AddAttribute(this XmlNode node, string attributeName, string value)
        {
            if (node.OwnerDocument != null)
            {
                XmlAttribute att = node.OwnerDocument.CreateAttribute(attributeName);
                att.Value = value;
                node.Attributes?.Append(att);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="nodeName"></param>
        /// <param name="val"></param>
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
