using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CSharpSyntax
{
    internal static class ReflectionExtensions
    {
        public static T GetCustomAttribute<T>(this ICustomAttributeProvider type)
            where T : Attribute
        {
            return GetCustomAttribute<T>(type, true);
        }

        public static T GetCustomAttribute<T>(this ICustomAttributeProvider type, bool inherit)
            where T : Attribute
        {
            var attributes = type.GetCustomAttributes(typeof(T), inherit);

            return attributes.Length == 0 ? null : (T)attributes[0];
        }
    }
}
