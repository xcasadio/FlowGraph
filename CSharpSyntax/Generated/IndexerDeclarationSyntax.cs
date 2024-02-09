using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class IndexerDeclarationSyntax : BasePropertyDeclarationSyntax
    {
        private BracketedParameterListSyntax _parameterList;
        public BracketedParameterListSyntax ParameterList
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
        
        public IndexerDeclarationSyntax()
            : base(SyntaxKind.IndexerDeclaration)
        {
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var child in base.ChildNodes())
            {
                yield return child;
            }
            
            if (ParameterList != null)
                yield return ParameterList;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitIndexerDeclaration(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitIndexerDeclaration(this);
        }
    }
}
