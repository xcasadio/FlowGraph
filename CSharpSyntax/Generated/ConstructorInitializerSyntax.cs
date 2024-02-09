using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class ConstructorInitializerSyntax : SyntaxNode
    {
        private ArgumentListSyntax _argumentList;
        public ArgumentListSyntax ArgumentList
        {
            get { return _argumentList; }
            set
            {
                if (_argumentList != null)
                    RemoveChild(_argumentList);
                
                _argumentList = value;
                
                if (_argumentList != null)
                    AddChild(_argumentList);
            }
        }
        
        public ThisOrBase Kind { get; set; }
        
        public ConstructorInitializerSyntax()
            : base(SyntaxKind.ConstructorInitializer)
        {
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            if (ArgumentList != null)
                yield return ArgumentList;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitConstructorInitializer(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitConstructorInitializer(this);
        }
    }
}
