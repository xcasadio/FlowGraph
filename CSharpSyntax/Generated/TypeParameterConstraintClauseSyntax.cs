using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class TypeParameterConstraintClauseSyntax : SyntaxNode
    {
        public SyntaxList<TypeParameterConstraintSyntax> Constraints { get; private set; }
        
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
        
        public TypeParameterConstraintClauseSyntax()
            : base(SyntaxKind.TypeParameterConstraintClause)
        {
            Constraints = new SyntaxList<TypeParameterConstraintSyntax>(this);
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var item in Constraints)
            {
                yield return item;
            }
            
            if (Name != null)
                yield return Name;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitTypeParameterConstraintClause(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitTypeParameterConstraintClause(this);
        }
    }
}
