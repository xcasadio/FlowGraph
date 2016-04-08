using System.Xml;
using FlowGraphBase;
using NFluent;
using NUnit.Framework;

namespace FlowGraphBaseTest
{
    class XmlExtensionTest
    {
        [Test]
        public void Should_add_root_node()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.AddRootNode("root");
            Check.That(xmlDoc.ChildNodes[1]).IsEqualTo(rootNode);
            Check.That(xmlDoc.ChildNodes[1].Name).IsEqualTo(rootNode.Name);
        }

        [Test]
        public void Should_add_attribute_to_node()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.AddRootNode("root");
            rootNode.AddAttribute("attribute", "value");
            Check.That(rootNode.Attributes["attribute"].Value).IsEqualTo("value");
        }

        [Test]
        public void Should_add_node_text()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement rootNode = xmlDoc.AddRootNode("root");
            XmlElement nodeTxt = xmlDoc.CreateElementWithText("Text", "value");
            Check.That(nodeTxt.Name).IsEqualTo("Text");
            Check.That(nodeTxt.InnerText).IsEqualTo("value");
        }
    }
}
