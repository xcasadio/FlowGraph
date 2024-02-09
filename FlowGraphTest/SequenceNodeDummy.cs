
using CSharpSyntax;
using FlowGraph.Nodes;
using Newtonsoft.Json.Linq;

namespace FlowGraphTest;

public class SequenceNodeDummy : SequenceNode
{
    protected override void InitializeSlots()
    { }

    public override SequenceNode Copy()
    {
        throw new NotImplementedException();
    }

    public override NodeType NodeType => NodeType.Action;
    public override string Title => "SequenceNodeDummy";

    public override SyntaxNode GenerateAst()
    {
        return null;
    }
}