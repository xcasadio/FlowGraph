using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class TryStatementSyntax : StatementSyntax
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
        
        public SyntaxList<CatchClauseSyntax> Catches { get; private set; }
        
        private FinallyClauseSyntax _finally;
        public FinallyClauseSyntax Finally
        {
            get { return _finally; }
            set
            {
                if (_finally != null)
                    RemoveChild(_finally);
                
                _finally = value;
                
                if (_finally != null)
                    AddChild(_finally);
            }
        }
        
        public TryStatementSyntax()
            : base(SyntaxKind.TryStatement)
        {
            Catches = new SyntaxList<CatchClauseSyntax>(this);
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var child in base.ChildNodes())
            {
                yield return child;
            }
            
            if (Block != null)
                yield return Block;
            
            foreach (var item in Catches)
            {
                yield return item;
            }
            
            if (Finally != null)
                yield return Finally;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitTryStatement(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitTryStatement(this);
        }
    }
}
