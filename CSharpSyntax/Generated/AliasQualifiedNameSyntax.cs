using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class AliasQualifiedNameSyntax : NameSyntax
    {
        private IdentifierNameSyntax _alias;
        public IdentifierNameSyntax Alias
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
        
        private SimpleNameSyntax _name;
        public SimpleNameSyntax Name
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
        
        public AliasQualifiedNameSyntax()
            : base(SyntaxKind.AliasQualifiedName)
        {
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var child in base.ChildNodes())
            {
                yield return child;
            }
            
            if (Alias != null)
                yield return Alias;
            
            if (Name != null)
                yield return Name;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitAliasQualifiedName(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitAliasQualifiedName(this);
        }
    }
}
