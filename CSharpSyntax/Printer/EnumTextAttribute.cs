using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpSyntax.Printer
{
    [AttributeUsage(AttributeTargets.Field)]
    internal class EnumTextAttribute : Attribute
    {
        public string Text { get; private set; }

        public EnumTextAttribute(string text)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            Text = text;
        }
    }
}
