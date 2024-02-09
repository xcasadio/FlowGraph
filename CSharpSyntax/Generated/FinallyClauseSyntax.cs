using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class FinallyClauseSyntax : SyntaxNode
    {
        private BlockSyntax _block;
        public BlockSyntax Block
        {
            get { return _block; }
            set
            {
                if (_block != null)
                    RemoveChild(_block);
                
                _block = value;
                
                if (_block != null)
                    AddChild(_block);
            }
        }
        
        public FinallyClauseSyntax()
            : base(SyntaxKind.FinallyClause)
        {
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            if (Block != null)
                yield return Block;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitFinallyClause(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitFinallyClause(this);
        }
    }
}
