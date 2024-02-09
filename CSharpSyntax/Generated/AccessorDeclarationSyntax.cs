using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class AccessorDeclarationSyntax : SyntaxTriviaNode
    {
        public SyntaxList<AttributeListSyntax> AttributeLists { get; private set; }
        
        private BlockSyntax _body;
        public BlockSyntax Body
        {
            get { return _body; }
            set
            {
                if (_body != null)
                    RemoveChild(_body);
                
                _body = value;
                
                if (_body != null)
                    AddChild(_body);
            }
        }
        
        public AccessorDeclarationKind Kind { get; set; }
        
        public Modifiers Modifiers { get; set; }
        
        public AccessorDeclarationSyntax()
            : base(SyntaxKind.AccessorDeclaration)
        {
            AttributeLists = new SyntaxList<AttributeListSyntax>(this);
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var item in AttributeLists)
            {
                yield return item;
            }
            
            if (Body != null)
                yield return Body;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitAccessorDeclaration(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitAccessorDeclaration(this);
        }
    }
}
