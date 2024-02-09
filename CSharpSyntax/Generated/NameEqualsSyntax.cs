using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class NameEqualsSyntax : SyntaxNode
    {
        private IdentifierNameSyntax _name;
        public IdentifierNameSyntax Name
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
        
        public NameEqualsSyntax()
            : base(SyntaxKind.NameEquals)
        {
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            if (Name != null)
                yield return Name;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitNameEquals(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitNameEquals(this);
        }
    }
}
