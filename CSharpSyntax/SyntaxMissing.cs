using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpSyntax
{
    partial class TypeSyntax
    {
        public virtual bool IsVar
        {
            get { return false; }
        }
    }

    partial class NameSyntax
    {
        public virtual int Arity
        {
            get { return 0; }
        }
    }

    partial class IdentifierNameSyntax
    {
        public override bool IsVar
        {
            get { return Identifier == "var"; }
        }
    }

    partial class ArrayRankSpecifierSyntax
    {
        public int Rank
        {
            get { return Sizes.Count; }
        }
    }

    partial class AccessorDeclarationSyntax : IHasAttributeLists
    {
    }

    partial class BaseFieldDeclarationSyntax : IHasAttributeLists
    {
    }

    partial class BaseMethodDeclarationSyntax : IHasAttributeLists
    {
    }

    partial class BasePropertyDeclarationSyntax : IHasAttributeLists
    {
    }

    partial class BaseTypeDeclarationSyntax : IHasAttributeLists
    {
    }

    partial class CompilationUnitSyntax : IHasAttributeLists
    {
    }

    partial class DelegateDeclarationSyntax : IHasAttributeLists
    {
    }

    partial class EnumMemberDeclarationSyntax : IHasAttributeLists
    {
    }

    partial class ParameterSyntax : IHasAttributeLists
    {
    }

    partial class TypeParameterSyntax : IHasAttributeLists
    {
    }
}
