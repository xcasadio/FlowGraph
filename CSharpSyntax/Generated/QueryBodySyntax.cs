using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class QueryBodySyntax : SyntaxNode
    {
        public SyntaxList<QueryClauseSyntax> Clauses { get; private set; }
        
        private QueryContinuationSyntax _continuation;
        public QueryContinuationSyntax Continuation
        {
            get { return _continuation; }
            set
            {
                if (_continuation != null)
                    RemoveChild(_continuation);
                
                _continuation = value;
                
                if (_continuation != null)
                    AddChild(_continuation);
            }
        }
        
        private SelectOrGroupClauseSyntax _selectOrGroup;
        public SelectOrGroupClauseSyntax SelectOrGroup
        {
            get { return _selectOrGroup; }
            set
            {
                if (_selectOrGroup != null)
                    RemoveChild(_selectOrGroup);
                
                _selectOrGroup = value;
                
                if (_selectOrGroup != null)
                    AddChild(_selectOrGroup);
            }
        }
        
        public QueryBodySyntax()
            : base(SyntaxKind.QueryBody)
        {
            Clauses = new SyntaxList<QueryClauseSyntax>(this);
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var item in Clauses)
            {
                yield return item;
            }
            
            if (Continuation != null)
                yield return Continuation;
            
            if (SelectOrGroup != null)
                yield return SelectOrGroup;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitQueryBody(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitQueryBody(this);
        }
    }
}
