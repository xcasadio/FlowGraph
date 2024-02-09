using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class AttributeSyntax : SyntaxNode
    {
        private AttributeArgumentListSyntax _argumentList;
        public AttributeArgumentListSyntax ArgumentList
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
        
        private NameSyntax _name;
        public NameSyntax Name
        {
            get { return _name; }
            set
            {
                if (_name != null)
                    RemoveChild(_name);
                
                _name = value;
                
                if (_name != null)
                    AddChild(_name);
            }
        }
        
        public AttributeSyntax()
            : base(SyntaxKind.Attribute)
        {
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            if (ArgumentList != null)
                yield return ArgumentList;
            
            if (Name != null)
                yield return Name;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitAttribute(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitAttribute(this);
        }
    }
}
