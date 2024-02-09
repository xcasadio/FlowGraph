using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class OperatorDeclarationSyntax : BaseMethodDeclarationSyntax
    {
        public Operator Operator { get; set; }
        
        private TypeSyntax _returnType;
        public TypeSyntax ReturnType
        {
            get { return _returnType; }
            set
            {
                if (_returnType != null)
                    RemoveChild(_returnType);
                
                _returnType = value;
                
                if (_returnType != null)
                    AddChild(_returnType);
            }
        }
        
        public OperatorDeclarationSyntax()
            : base(SyntaxKind.OperatorDeclaration)
        {
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var child in base.ChildNodes())
            {
                yield return child;
            }
            
            if (ReturnType != null)
                yield return ReturnType;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitOperatorDeclaration(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitOperatorDeclaration(this);
        }
    }
}
