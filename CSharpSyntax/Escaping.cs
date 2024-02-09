using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CSharpSyntax
{
    internal static class Escaping
    {
        public static string StringEncode(string value, char quoteChar, bool enclose)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            Debug.Assert(quoteChar == '\'' || quoteChar == '"');

            var sb = new StringBuilder();

            if (enclose)
                sb.Append(quoteChar);

            for (int i = 0; i < value.Length; i++)
            {
                switch (value[i])
                {
                    case '\r': sb.Append("\\r"); break;
                    case '\n': sb.Append("\\n"); break;
                    case '\t': sb.Append("\\t"); break;
                    case '\'': sb.Append("\\'"); break;
                    case '"': sb.Append("\\\""); break;
                    case '\\': sb.Append("\\\\"); break;
                    default:
                        if (Char.IsControl(value[i]))
                        {
                            sb.Append("\\u");
                            sb.Append(((int)value[i]).ToString("X4"));
                        }
                        else
                        {
                            sb.Append(value[i]);
                        }
                        break;
                }
            }

            if (enclose)
                sb.Append(quoteChar);

            return sb.ToString();
        }
    }
}
