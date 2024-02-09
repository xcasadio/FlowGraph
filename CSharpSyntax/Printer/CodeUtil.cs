using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CSharpSyntax.Printer
{
    internal static class CodeUtil
    {
        public static string Encode(object value)
        {
            if (value is Enum)
                throw new InvalidOperationException("Cannot encode enum value");

            if (value == null)
                return "null";

            switch (Type.GetTypeCode(value.GetType()))
            {
                case TypeCode.Boolean:
                    if ((bool)value)
                        return "true";
                    return "false";

                case TypeCode.Char:
                    return Escaping.StringEncode(new string(new[] { (char)value }), '\'', true);

                case TypeCode.DBNull:
                    return "DBNull.Value";

                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return Convert.ToString(value, CultureInfo.InvariantCulture);

                case TypeCode.DateTime:
                    var dateTime = (DateTime)value;

                    if (
                        dateTime.Hour == 0 &&
                        dateTime.Minute == 0 &&
                        dateTime.Second == 0
                    )
                        return String.Format("new DateTime({0}, {1}, {2})", dateTime.Year, dateTime.Month, dateTime.Day);

                    return String.Format(
                        "new DateTime({0}, {1}, {2}, {3}, {4}, {5})",
                        dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second
                    );

                case TypeCode.Empty:
                    return "null";

                case TypeCode.String:
                    return Escaping.StringEncode((string)value, '"', true);

                default:
                    throw new InvalidOperationException("Cannot encode type " + value.GetType());
            }
        }
    }
}
