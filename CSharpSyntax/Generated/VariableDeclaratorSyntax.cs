using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class VariableDeclaratorSyntax : SyntaxNode
    {
        private BracketedArgumentListSyntax _argumentList;
        public BracketedArgumentListSyntax ArgumentList
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
        
        public string Identifier { get; set; }
        
        private EqualsValueClauseSyntax _initializer;
        public EqualsValueClauseSyntax Initializer
        {
            get { return _initializer; }
            set
            {
                if (_initializer != null)
                    RemoveChild(_initializer);
                
                _initializer = value;
                
                if (_initializer != null)
                    AddChild(_initializer);
            }
        }
        
        public VariableDeclaratorSyntax()
            : base(SyntaxKind.VariableDeclarator)
        {
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            if (ArgumentList != null)
                yield return ArgumentList;
            
            if (Initializer != null)
                yield return Initializer;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitVariableDeclarator(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitVariableDeclarator(this);
        }
    }
}
