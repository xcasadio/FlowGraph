using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class VariableDeclarationSyntax : SyntaxNode
    {
        private TypeSyntax _type;
        public TypeSyntax Type
        {
            get { return _type; }
            set
            {
                if (_type != null)
                    RemoveChild(_type);
                
                _type = value;
                
                if (_type != null)
                    AddChild(_type);
            }
        }
        
        public SyntaxList<VariableDeclaratorSyntax> Variables { get; private set; }
        
        public VariableDeclarationSyntax()
            : base(SyntaxKind.VariableDeclaration)
        {
            Variables = new SyntaxList<VariableDeclaratorSyntax>(this);
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            if (Type != null)
                yield return Type;
            
            foreach (var item in Variables)
            {
                yield return item;
            }
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitVariableDeclaration(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitVariableDeclaration(this);
        }
    }
}
