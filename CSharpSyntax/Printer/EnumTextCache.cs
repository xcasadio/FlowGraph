using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpSyntax.Printer
{
    internal class EnumTextCache<T>
        where T : struct
    {
        private readonly Dictionary<T, string> _texts = new Dictionary<T, string>();

        public EnumTextCache()
        {
            foreach (var name in Enum.GetNames(typeof(T)))
            {
                var field = typeof(T).GetField(name);

                var attribute = field.GetCustomAttribute<EnumTextAttribute>();

                if (attribute == null)
                {
                    throw new InvalidOperationException(String.Format(
                        "Field '{0}' of enum type '{1}' does not specify an enum text",
                        name, typeof(T)
                    ));
                }

                _texts.Add((T)Enum.Parse(typeof(T), name), attribute.Text);
            }
        }

        public string this[T value]
        {
            get
            {
                return _texts[value];
            }
        }
    }
}
