using System.Xml;
using DotNetCodeGenerator.Ast;
using FlowGraph.Attributes;
using FlowGraph.Process;

namespace FlowGraph.Nodes;

[Category("Event")]
public abstract class EventNode : SequenceNode
{
#if EDITOR
    public override NodeType NodeType => NodeType.Event;

    protected EventNode()
    {
    }

    public override Statement GenerateAst()
    {
        var block = new Scope();

        foreach (var slot in SlotConnectorOut)
        {
            foreach (var node in slot.ConnectedNodes)
            {
                block.Statements.Add(node.Node.GenerateAst());
            }
        }

        return new FunctionDeclaration("function" + Title, Enumerable.Empty<Token>(), block);
    }

#endif

    protected EventNode(XmlNode node)
        : base(node)
    {
    }

    protected override void InitializeSlots()
    {
        SlotFlag = SlotAvailableFlag.DefaultFlagEvent;
    }

    public void Triggered(ProcessingContext context, int index, object? para)
    {
        TriggeredImpl(para);
        ActivateOutputLink(context, index);
    }

    protected abstract void TriggeredImpl(object? para);
}