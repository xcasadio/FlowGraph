using System;
using System.Xml;

namespace FlowGraphBase
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
        /// <param name="xmlNode"></param>
        /// <param name="attributeName"></param>
        /// <param name="value"></param>
        public static void AddAttribute(this XmlNode xmlNode, string attributeName, string value)
        {
            if (xmlNode.OwnerDocument != null)
            {
                XmlAttribute att = xmlNode.OwnerDocument.CreateAttribute(attributeName);
                att.Value = value;
                xmlNode.Attributes.Append(att);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="nodeName"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static XmlElement CreateElementWithText(this XmlDocument xmlDoc, string nodeName, string val)
        {
            XmlElement el = xmlDoc.CreateElement(nodeName);
            XmlText txtXml = xmlDoc.CreateTextNode(val);
            el.AppendChild(txtXml);

            return el;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static object LoadValue(this XmlNode node)
        {
            if (node.Attributes != null)
            {
                string typeValue = node.Attributes["valueType"].Value;

                if (string.Equals(typeValue, "null"))
                {
                    return null;
                }

                Type type = Type.GetType(typeValue);
                return Convert.ChangeType(node.InnerText, type);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node_"></param>
        /// <param name="value_"></param>
        public static void SaveValue(this XmlNode node, object value)
        {
            if (value != null)
            {
                string typeName = value.GetType().AssemblyQualifiedName;
                int index = typeName.IndexOf(',', typeName.IndexOf(',') + 1);
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
