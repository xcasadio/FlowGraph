using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpSyntax
{
    public abstract partial class BaseMethodDeclarationSyntax : MemberDeclarationSyntax
    {
        public SyntaxList<AttributeListSyntax> AttributeLists { get; private set; }
        
        private BlockSyntax _body;
        public BlockSyntax Body
        {
            get { return _body; }
            set
            {
                if (_body != null)
                    RemoveChild(_body);
                
                _body = value;
                
                if (_body != null)
                    AddChild(_body);
            }
        }
        
        public Modifiers Modifiers { get; set; }
        
        private ParameterListSyntax _parameterList;
        public ParameterListSyntax ParameterList
        {
            get { return _parameterList; }
            set
            {
                if (_parameterList != null)
                    RemoveChild(_parameterList);
                
                _parameterList = value;
                
                if (_parameterList != null)
                    AddChild(_parameterList);
            }
        }
        
        internal BaseMethodDeclarationSyntax(SyntaxKind syntaxKind)
            : base(syntaxKind)
        {
            AttributeLists = new SyntaxList<AttributeListSyntax>(this);
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var child in base.ChildNodes())
            {
                yield return child;
            }
            
            foreach (var item in AttributeLists)
            {
                yield return item;
            }
            
            if (Body != null)
                yield return Body;
            
            if (ParameterList != null)
                yield return ParameterList;
        }
    }
}
