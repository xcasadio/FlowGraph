using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class CatchClauseSyntax : SyntaxNode
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
        
        private CatchDeclarationSyntax _declaration;
        public CatchDeclarationSyntax Declaration
        {
            get { return _declaration; }
            set
            {
                if (_declaration != null)
                    RemoveChild(_declaration);
                
                _declaration = value;
                
                if (_declaration != null)
                    AddChild(_declaration);
            }
        }
        
        public CatchClauseSyntax()
            : base(SyntaxKind.CatchClause)
        {
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            if (Block != null)
                yield return Block;
            
            if (Declaration != null)
                yield return Declaration;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitCatchClause(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitCatchClause(this);
        }
    }
}
