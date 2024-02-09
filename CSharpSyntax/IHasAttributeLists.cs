using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpSyntax
{
    public interface IHasAttributeLists
    {
        SyntaxList<AttributeListSyntax> AttributeLists { get; }
    }
}
