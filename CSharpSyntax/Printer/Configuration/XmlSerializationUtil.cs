using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CSharpSyntax.Printer.Configuration
{
    internal static class XmlSerializationUtil
    {
        public static void AssignDefaultValues(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            foreach (var property in obj.GetType().GetProperties())
            {
                var defaultValueAttribute = property.GetCustomAttribute<DefaultValueAttribute>();

                if (defaultValueAttribute != null)
                    property.SetValue(obj, defaultValueAttribute.Value, null);
            }
        }
    }
}
