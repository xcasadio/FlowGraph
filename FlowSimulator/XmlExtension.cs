using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Globalization;

namespace FlowSimulator
{
    /// <summary>
    /// 
    /// </summary>
    static public class XMLExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDoc_"></param>
        /// <param name="nodeName_"></param>
        /// <returns>the root node added</returns>
        static public XmlElement AddRootNode(this XmlDocument xmlDoc_, string nodeName_)
        {
            //let's add the XML declaration section
            XmlNode xmlnode = xmlDoc_.CreateNode(XmlNodeType.XmlDeclaration, "", "");
            xmlDoc_.AppendChild(xmlnode);

            //let's add the root element
            XmlElement xmlElem = xmlDoc_.CreateElement("", nodeName_, "");
            xmlDoc_.AppendChild(xmlElem);

            return xmlElem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        /// <param name="attributeName_"></param>
        /// <param name="value_"></param>
        static public void AddAttribute(this XmlNode node_, string attributeName_, string value_)
        {
            XmlAttribute att = node_.OwnerDocument.CreateAttribute(attributeName_);
            att.Value = value_;
            node_.Attributes.Append(att);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        /// <param name="nodeName_"></param>
        /// <param name="val_"></param>
        static public void AddElementWithText(this XmlNode node_, string nodeName_, string val_)
        {
            XmlElement el = node_.OwnerDocument.CreateElement(nodeName_);
            XmlText txtXML = node_.OwnerDocument.CreateTextNode(val_);
            el.AppendChild(txtXML);
        }        
    }
}
