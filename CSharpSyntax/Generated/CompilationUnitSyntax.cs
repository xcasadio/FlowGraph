using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CSharpSyntax
{
    public sealed partial class CompilationUnitSyntax : SyntaxTriviaNode
    {
        public SyntaxList<AttributeListSyntax> AttributeLists { get; private set; }
        
        public SyntaxList<ExternAliasDirectiveSyntax> Externs { get; private set; }
        
        public SyntaxList<MemberDeclarationSyntax> Members { get; private set; }
        
        public SyntaxList<UsingDirectiveSyntax> Usings { get; private set; }
        
        public CompilationUnitSyntax()
            : base(SyntaxKind.CompilationUnit)
        {
            AttributeLists = new SyntaxList<AttributeListSyntax>(this);
            Externs = new SyntaxList<ExternAliasDirectiveSyntax>(this);
            Members = new SyntaxList<MemberDeclarationSyntax>(this);
            Usings = new SyntaxList<UsingDirectiveSyntax>(this);
        }
        
        public override IEnumerable<SyntaxNode> ChildNodes()
        {
            foreach (var item in AttributeLists)
            {
                yield return item;
            }
            
            foreach (var item in Externs)
            {
                yield return item;
            }
            
            foreach (var item in Members)
            {
                yield return item;
            }
            
            foreach (var item in Usings)
            {
                yield return item;
            }
        }
        
        [DebuggerStepThrough]
        public override void Accept(ISyntaxVisitor visitor)
        {
            if (!visitor.Done)
                visitor.VisitCompilationUnit(this);
        }
        
        [DebuggerStepThrough]
        public override T Accept<T>(ISyntaxVisitor<T> visitor)
        {
            return visitor.VisitCompilationUnit(this);
        }
    }
}
