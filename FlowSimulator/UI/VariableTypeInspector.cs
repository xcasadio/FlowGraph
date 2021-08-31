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
        private static readonly List<Type> _Types = new List<Type>();

        /// <summary>
        /// 
        /// </summary>
        public static IEnumerable<Type> Types => _Types;

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
            _Types.Add(typeof(bool));
            _Types.Add(typeof(sbyte));
            _Types.Add(typeof(char)); 
            _Types.Add(typeof(short));
            _Types.Add(typeof(int));
            _Types.Add(typeof(long));
            _Types.Add(typeof(byte));
            _Types.Add(typeof(ushort));
            _Types.Add(typeof(uint));
            _Types.Add(typeof(ulong));
            _Types.Add(typeof(float));
            _Types.Add(typeof(double));
            _Types.Add(typeof(string));
            _Types.Add(typeof(object));
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
        public static Color GetColorFromType(Type type_)
        {
            if (type_ == typeof(bool))
            {
                return BooleanColor;
            }

            if (type_ == typeof(sbyte)
                || type_ == typeof(char)
                || type_ == typeof(short)
                || type_ == typeof(int)
                || type_ == typeof(long)
                || type_ == typeof(byte)
                || type_ == typeof(ushort)
                || type_ == typeof(uint)
                || type_ == typeof(ulong))
            {
                return IntegerColor;
            }
            if (type_ == typeof(float)
                || type_ == typeof(double))
            {
                return IntegerColor;
            }
            if (type_ == typeof(string))
            {
                return StringColor;
            }
            if (type_ == typeof(object))
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
        public static bool CheckCompatibilityType(Type a_, Type b_)
        {
            if (a_ == typeof(float)
                || a_ == typeof(double))
            {
                return b_ == typeof(float)
                        || b_ == typeof(double);
            }

            return a_ == b_;
        }
    }
}
