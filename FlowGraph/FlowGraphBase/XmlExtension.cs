using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        /// <param name="xmlDoc_"></param>
        /// <param name="nodeName_"></param>
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
        /// <param name="xmlDoc_"></param>
        /// <param name="xmlElement_"></param>
        /// <param name="attributeName_"></param>
        /// <param name="value_"></param>
        static public void AddAttribute(this XmlNode xmlNode_, string attributeName_, string value_)
        {
            XmlAttribute att = xmlNode_.OwnerDocument.CreateAttribute(attributeName_);
            att.Value = value_;
            xmlNode_.Attributes.Append(att);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDoc_"></param>
        /// <param name="nodeName_"></param>
        /// <param name="val_"></param>
        /// <returns></returns>
        static public XmlElement CreateElementWithText(this XmlDocument xmlDoc_, string nodeName_, string val_)
        {
            XmlElement el = xmlDoc_.CreateElement(nodeName_);
            XmlText txtXML = xmlDoc_.CreateTextNode(val_);
            el.AppendChild(txtXML);

            return el;
        }

        #region Save/Load value

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node_"></param>
        /// <returns></returns>
        //         static public T LoadValue<T>(this XmlNode node_) where T : class
        //         {
        //             string typeValue = node_.Attributes["valueType"].Value;
        //             
        //             if (string.Equals(typeValue, "null") == true)
        //             {
        //                 return null;
        //             }
        //             else if (string.Equals(typeValue, typeof(MessageDatas).FullName) == true)
        //             {
        //                 return null;
        //             }
        //             else if (string.Equals(typeValue, typeof(Session).FullName) == true)
        //             {
        //                 return null;
        //             }
        //             else if (string.Equals(typeValue, typeof(int).FullName) == true)
        //             {
        //                 return null;
        //             }
        //             else if (string.Equals(typeValue, typeof(long).FullName) == true)
        //             {
        //                 return null;
        //             }
        //             else if (string.Equals(typeValue, typeof(float).FullName) == true)
        //             {
        //                 return null;
        //             }
        //             else if (string.Equals(typeValue, typeof(double).FullName) == true)
        //             {
        //                 return null;
        //             }
        // 
        //             return (T)Convert.ChangeType(node_.InnerText, typeof(T));
        //         }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        /// <returns></returns>
        static public object LoadValue(this XmlNode node_)
        {
            string typeValue = node_.Attributes["valueType"].Value;

            if (string.Equals(typeValue, "null") == true)
            {
                return null;
            }

            Type type = Type.GetType(typeValue);
            return Convert.ChangeType(node_.InnerText, type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node_"></param>
        /// <param name="value_"></param>
        static public void SaveValue(this XmlNode node_, object value_)
        {
            if (value_ != null)
            {
                string typeName = value_.GetType().AssemblyQualifiedName;
                int index = typeName.IndexOf(',', typeName.IndexOf(',') + 1);
                typeName = typeName.Substring(0, index);
                node_.AddAttribute("valueType", typeName);
                node_.InnerText = Convert.ToString(value_);
            }
            else
            {
                node_.AddAttribute("valueType", "null");
            }
        }

        #endregion // XmlNode Extension
    }
}
