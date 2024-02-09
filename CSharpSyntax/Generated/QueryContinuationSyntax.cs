using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class QueryContinuationSyntax : SyntaxNode
    {
        private QueryBodySyntax _body;
        public QueryBodySyntax Body
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
        
        public string Identifier { get; set; }
        
        public QueryContinuationSyntax()
            : base(SyntaxKind.QueryContinuation)
        {
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            if (Body != null)
                yield return Body;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitQueryContinuation(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitQueryContinuation(this);
        }
    }
}
