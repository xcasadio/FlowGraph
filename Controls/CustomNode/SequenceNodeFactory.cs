using FlowGraph.Nodes;
using FlowGraph.Nodes.Actions.Math;
using FlowGraph.Nodes.Variables;
using Newtonsoft.Json.Linq;

namespace CustomNode
{
    public class SequenceNodeFactory : ISequenceNodeFactory
    {
        public SequenceNode? CreateNode(JObject jObject)
        {
            var nodeAttribute = jObject["type"];
            if (nodeAttribute == null)
            {
                return null;
            }

            var typeName = nodeAttribute.Value<string>();
            SequenceNode node = null;

            if (typeName.Equals(typeof(AdditionNodeInt).FullName))
            {
                node = new AdditionNodeInt();
            }
            else if (typeName.Equals(typeof(LogMessageNode).FullName))
            {
                node = new LogMessageNode();
            }
            else if (typeName.Equals(typeof(EventTestStartedNode).FullName))
            {
                node = new EventTestStartedNode();
            }
            else if (typeName.Equals(typeof(EventTaskStartedNode).FullName))
            {
                node = new EventTaskStartedNode();
            }
            else if (typeName.Equals(typeof(EventMessageReceivedNode).FullName))
            {
                node = new EventMessageReceivedNode();
            }
            else if (typeName.Equals(typeof(VariableNodeInt).FullName))
            {
                node = new VariableNodeInt();
            }
            else
            {
                throw new ArgumentException($"The type {typeName} is not supported");
            }

            node.Load(jObject);

            return node;
        }
    }
}
