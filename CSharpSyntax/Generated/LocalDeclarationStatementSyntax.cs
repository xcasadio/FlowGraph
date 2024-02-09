using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class LocalDeclarationStatementSyntax : StatementSyntax
    {
        private VariableDeclarationSyntax _declaration;
        public VariableDeclarationSyntax Declaration
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
        
        public Modifiers Modifiers { get; set; }
        
        public LocalDeclarationStatementSyntax()
            : base(SyntaxKind.LocalDeclarationStatement)
        {
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var child in base.ChildNodes())
            {
                yield return child;
            }
            
            if (Declaration != null)
                yield return Declaration;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitLocalDeclarationStatement(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitLocalDeclarationStatement(this);
        }
    }
}
