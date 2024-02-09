using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class AttributeArgumentSyntax : SyntaxNode
    {
        private ExpressionSyntax _expression;
        public ExpressionSyntax Expression
        {
            get { return _expression; }
            set
            {
                if (_expression != null)
                    RemoveChild(_expression);
                
                _expression = value;
                
                if (_expression != null)
                    AddChild(_expression);
            }
        }
        
        private NameColonSyntax _nameColon;
        public NameColonSyntax NameColon
        {
            get { return _nameColon; }
            set
            {
                if (_nameColon != null)
                    RemoveChild(_nameColon);
                
                _nameColon = value;
                
                if (_nameColon != null)
                    AddChild(_nameColon);
            }
        }
        
        private NameEqualsSyntax _nameEquals;
        public NameEqualsSyntax NameEquals
        {
            get { return _nameEquals; }
            set
            {
                if (_nameEquals != null)
                    RemoveChild(_nameEquals);
                
                _nameEquals = value;
                
                if (_nameEquals != null)
                    AddChild(_nameEquals);
            }
        }
        
        public AttributeArgumentSyntax()
            : base(SyntaxKind.AttributeArgument)
        {
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            if (Expression != null)
                yield return Expression;
            
            if (NameColon != null)
                yield return NameColon;
            
            if (NameEquals != null)
                yield return NameEquals;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitAttributeArgument(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitAttributeArgument(this);
        }
    }
}
