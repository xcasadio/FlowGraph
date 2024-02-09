using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpSyntax
{
    [Flags]
    public enum Modifiers
    {
        None = 0,
        Async = 1 << 0,
        Abstract = 1 << 1,
        Const = 1 << 2,
        Extern = 1 << 3,
        Internal = 1 << 4,
        New = 1 << 5,
        Override = 1 << 6,
        Partial = 1 << 7,
        Private = 1 << 8,
        Protected = 1 << 9,
        Public = 1 << 10,
        ReadOnly = 1 << 11,
        Sealed = 1 << 12,
        Static = 1 << 13,
        Virtual = 1 << 14,
        Volatile = 1 << 15,
        All = (1 << 16) - 1
    }
}
