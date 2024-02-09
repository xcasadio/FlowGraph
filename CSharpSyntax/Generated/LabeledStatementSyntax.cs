using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class LabeledStatementSyntax : StatementSyntax
    {
        public string Identifier { get; set; }
        
        private StatementSyntax _statement;
        public StatementSyntax Statement
        {
            get { return _statement; }
            set
            {
                if (_statement != null)
                    RemoveChild(_statement);
                
                _statement = value;
                
                if (_statement != null)
                    AddChild(_statement);
            }
        }
        
        public LabeledStatementSyntax()
            : base(SyntaxKind.LabeledStatement)
        {
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var child in base.ChildNodes())
            {
                yield return child;
            }
            
            if (Statement != null)
                yield return Statement;
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitLabeledStatement(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitLabeledStatement(this);
        }
    }
}
