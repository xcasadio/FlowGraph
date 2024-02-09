using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class AnonymousMethodExpressionSyntax : ExpressionSyntax
    {
        private BlockSyntax _block;
        public BlockSyntax Block
        {
            get { return _block; }
            set
            {
                if (_block != null)
                    RemoveChild(_block);
                
                _block = value;
                
                if (_block != null)
                    AddChild(_block);
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
        
        public AnonymousMethodExpressionSyntax()
            : base(SyntaxKind.AnonymousMethodExpression)
        {
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var child in base.ChildNodes())
            {
                yield return child;
            }
            
            if (Block != null)
                yield return Block;
            
            if (ParameterList != null)
                yield return ParameterList;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitAnonymousMethodExpression(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitAnonymousMethodExpression(this);
        }
    }
}
