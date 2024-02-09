using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class SwitchSectionSyntax : SyntaxTriviaNode
    {
        public SyntaxList<SwitchLabelSyntax> Labels { get; private set; }
        
        public SyntaxList<StatementSyntax> Statements { get; private set; }
        
        public SwitchSectionSyntax()
            : base(SyntaxKind.SwitchSection)
        {
            Labels = new SyntaxList<SwitchLabelSyntax>(this);
            Statements = new SyntaxList<StatementSyntax>(this);
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var item in Labels)
            {
                yield return item;
            }
            
            foreach (var item in Statements)
            {
                yield return item;
            }
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitSwitchSection(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitSwitchSection(this);
        }
    }
}
