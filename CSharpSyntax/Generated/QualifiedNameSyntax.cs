using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class QualifiedNameSyntax : NameSyntax
    {
        private NameSyntax _left;
        public NameSyntax Left
        {
            get { return _left; }
            set
            {
                if (_left != null)
                    RemoveChild(_left);
                
                _left = value;
                
                if (_left != null)
                    AddChild(_left);
            }
        }
        
        private SimpleNameSyntax _right;
        public SimpleNameSyntax Right
        {
            get { return _right; }
            set
            {
                if (_right != null)
                    RemoveChild(_right);
                
                _right = value;
                
                if (_right != null)
                    AddChild(_right);
            }
        }
        
        public QualifiedNameSyntax()
            : base(SyntaxKind.QualifiedName)
        {
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var child in base.ChildNodes())
            {
                yield return child;
            }
            
            if (Left != null)
                yield return Left;
            
            if (Right != null)
                yield return Right;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitQualifiedName(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitQualifiedName(this);
        }
    }
}
