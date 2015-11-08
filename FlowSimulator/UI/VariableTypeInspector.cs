using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace FlowSimulator.UI
{
    /// <summary>
    /// 
    /// </summary>
    static public class VariableTypeInspector
    {
		#region Fields

        static private List<Type> m_Types = new List<Type>();

		#endregion //Fields
	
		#region Properties

        /// <summary>
        /// 
        /// </summary>
        static public IEnumerable<Type> Types
        {
            get { return m_Types; }
        }

        /// <summary>
        /// 
        /// </summary>
        static public Color BooleanColor
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        static public Color IntegerColor
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        static public Color DoubleColor
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        static public Color StringColor
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        static public Color ObjectColor
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        static public Color MessageColor
        {
            get;
            set;
        }

		#endregion //Properties
	
		#region Constructors

        /// <summary>
        /// 
        /// </summary>
        static VariableTypeInspector()
        {
            m_Types.Add(typeof(bool));
            m_Types.Add(typeof(sbyte));
            m_Types.Add(typeof(char)); 
            m_Types.Add(typeof(short));
            m_Types.Add(typeof(int));
            m_Types.Add(typeof(long));
            m_Types.Add(typeof(byte));
            m_Types.Add(typeof(ushort));
            m_Types.Add(typeof(uint));
            m_Types.Add(typeof(ulong));
            m_Types.Add(typeof(float));
            m_Types.Add(typeof(double));
            m_Types.Add(typeof(string));
            m_Types.Add(typeof(object));
        }

		#endregion //Constructors
	
		#region Methods
		
        /// <summary>
        /// 
        /// </summary>
        static public void SetDefaultValues()
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
        static public Color GetColorFromType(Type type_)
        {
            if (type_ == typeof(bool))
            {
                return BooleanColor;
            }
            else if (type_ == typeof(sbyte)
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
            else if (type_ == typeof(float)
                || type_ == typeof(double))
            {
                return IntegerColor;
            }
            else if (type_ == typeof(string))
            {
                return StringColor;
            }
            else if (type_ == typeof(object))
            {
                return ObjectColor;
            }

            return Colors.White;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type_"></param>
        static public object CreateDefaultValueFromType(Type type_)
        {
            if (type_ == typeof(string))
            {
                return string.Empty;
            }

            return Activator.CreateInstance(type_);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a_"></param>
        /// <param name="b_"></param>
        /// <returns></returns>
        static public bool CheckCompatibilityType(Type a_, Type b_)
        {
            if (a_ == typeof(float)
                || a_ == typeof(double))
            {
                return b_ == typeof(float)
                        || b_ == typeof(double);
            }

            return a_ == b_;
        }

		#endregion //Methods
    }
}
