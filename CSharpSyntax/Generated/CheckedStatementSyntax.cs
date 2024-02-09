using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class CheckedStatementSyntax : StatementSyntax
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
        
        public CheckedOrUnchecked Kind { get; set; }
        
        public CheckedStatementSyntax()
            : base(SyntaxKind.CheckedStatement)
        {
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var child in base.ChildNodes())
            {
                yield return child;
            }
            
            if (Block != null)
                yield return Block;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitCheckedStatement(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitCheckedStatement(this);
        }
    }
}
