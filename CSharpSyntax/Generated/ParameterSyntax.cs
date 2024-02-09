using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class ParameterSyntax : SyntaxNode
    {
        public SyntaxList<AttributeListSyntax> AttributeLists { get; private set; }
        
        private EqualsValueClauseSyntax _default;
        public EqualsValueClauseSyntax Default
        {
            get { return _default; }
            set
            {
                if (_default != null)
                    RemoveChild(_default);
                
                _default = value;
                
                if (_default != null)
                    AddChild(_default);
            }
        }
        
        public string Identifier { get; set; }
        
        public ParameterModifier Modifier { get; set; }
        
        private TypeSyntax _type;
        public TypeSyntax Type
        {
            get { return _type; }
            set
            {
                if (_type != null)
                    RemoveChild(_type);
                
                _type = value;
                
                if (_type != null)
                    AddChild(_type);
            }
        }
        
        public ParameterSyntax()
            : base(SyntaxKind.Parameter)
        {
            AttributeLists = new SyntaxList<AttributeListSyntax>(this);
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var item in AttributeLists)
            {
                yield return item;
            }
            
            if (Default != null)
                yield return Default;
            
            if (Type != null)
                yield return Type;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitParameter(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitParameter(this);
        }
    }
}
