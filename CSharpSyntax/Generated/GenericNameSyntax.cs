using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class GenericNameSyntax : SimpleNameSyntax
    {
        public bool IsUnboundGenericName { get; set; }
        
        private TypeArgumentListSyntax _typeArgumentList;
        public TypeArgumentListSyntax TypeArgumentList
        {
            get { return _typeArgumentList; }
            set
            {
                if (_typeArgumentList != null)
                    RemoveChild(_typeArgumentList);
                
                _typeArgumentList = value;
                
                if (_typeArgumentList != null)
                    AddChild(_typeArgumentList);
            }
        }
        
        public GenericNameSyntax()
            : base(SyntaxKind.GenericName)
        {
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var child in base.ChildNodes())
            {
                yield return child;
            }
            
            if (TypeArgumentList != null)
                yield return TypeArgumentList;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitGenericName(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitGenericName(this);
        }
    }
}
