using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpSyntax
{
    internal static class ThrowHelper
    {
        public static Exception InvalidEnumValue<T>(T value)
        {
            return new InvalidOperationException(String.Format("Value '{0}' is not valid for enum '{1}'", value, typeof(T)));
        }
    }
}
