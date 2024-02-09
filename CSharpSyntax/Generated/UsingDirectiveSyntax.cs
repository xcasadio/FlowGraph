using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class UsingDirectiveSyntax : SyntaxTriviaNode
    {
        private NameEqualsSyntax _alias;
        public NameEqualsSyntax Alias
        {
            get { return _alias; }
            set
            {
                if (_alias != null)
                    RemoveChild(_alias);
                
                _alias = value;
                
                if (_alias != null)
                    AddChild(_alias);
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
        
        public UsingDirectiveSyntax()
            : base(SyntaxKind.UsingDirective)
        {
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            if (Alias != null)
                yield return Alias;
            
            if (Name != null)
                yield return Name;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitUsingDirective(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitUsingDirective(this);
        }
    }
}
