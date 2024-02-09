using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpSyntax
{
    internal static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> self, IEnumerable<T> values)
        {
            foreach (var value in values)
            {
                self.Add(value);
            }
        }
    }
}
