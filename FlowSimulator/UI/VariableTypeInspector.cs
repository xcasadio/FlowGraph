using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace FlowSimulator.UI
{
    /// <summary>
    /// 
    /// </summary>
    public static class VariableTypeInspector
    {
        /// <summary>
        /// 
        /// </summary>
        public static IEnumerable<Type> Types
        {
            get;
        } = new List<Type>();

        /// <summary>
        /// 
        /// </summary>
        public static Color BooleanColor
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Color IntegerColor
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Color DoubleColor
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Color StringColor
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Color ObjectColor
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public static Color MessageColor
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        static VariableTypeInspector()
        {
            var types = (List<Type>)Types;
            types.Add(typeof(bool));
            types.Add(typeof(sbyte));
            types.Add(typeof(char));
            types.Add(typeof(short));
            types.Add(typeof(int));
            types.Add(typeof(long));
            types.Add(typeof(byte));
            types.Add(typeof(ushort));
            types.Add(typeof(uint));
            types.Add(typeof(ulong));
            types.Add(typeof(float));
            types.Add(typeof(double));
            types.Add(typeof(string));
            types.Add(typeof(object));
        }

        /// <summary>
        /// 
        /// </summary>
        public static void SetDefaultValues()
        {
            BooleanColor = Colors.Red;
            IntegerColor = Colors.Cyan;
            DoubleColor = Colors.Green;
            StringColor = Colors.Magenta;
            ObjectColor = Colors.Blue;
            MessageColor = Colors.Orange;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type_"></param>
        public static Color GetColorFromType(Type type)
        {
            if (type == typeof(bool))
            {
                return BooleanColor;
            }

            if (type == typeof(sbyte)
                || type == typeof(char)
                || type == typeof(short)
                || type == typeof(int)
                || type == typeof(long)
                || type == typeof(byte)
                || type == typeof(ushort)
                || type == typeof(uint)
                || type == typeof(ulong))
            {
                return IntegerColor;
            }
            if (type == typeof(float)
                || type == typeof(double))
            {
                return IntegerColor;
            }
            if (type == typeof(string))
            {
                return StringColor;
            }
            if (type == typeof(object))
            {
                return ObjectColor;
            }

            return Colors.White;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type_"></param>
        public static object CreateDefaultValueFromType(Type type)
        {
            if (type == typeof(string))
            {
                return string.Empty;
            }

            return Activator.CreateInstance(type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a_"></param>
        /// <param name="b_"></param>
        /// <returns></returns>
        public static bool CheckCompatibilityType(Type a, Type b)
        {
            if (a == typeof(float) || a == typeof(double))
            {
                return b == typeof(float) || b == typeof(double);
            }

            return a == b;
        }
    }
}
