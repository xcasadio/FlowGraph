using System.Windows.Media;

namespace FlowGraphUI;

public static class VariableTypeInspector
{
    public static IEnumerable<Type> Types { get; } = new List<Type>();

    public static Color BooleanColor { get; set; }

    public static Color IntegerColor { get; set; }

    public static Color DoubleColor { get; set; }

    public static Color StringColor { get; set; }

    public static Color ObjectColor { get; set; }

    public static Color MessageColor
    {
        get; set;
    }

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

    public static void SetDefaultValues()
    {
        BooleanColor = Colors.Red;
        IntegerColor = Colors.Cyan;
        DoubleColor = Colors.Green;
        StringColor = Colors.Magenta;
        ObjectColor = Colors.Blue;
        MessageColor = Colors.Orange;
    }

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

    public static object CreateDefaultValueFromType(Type type)
    {
        if (type == typeof(string))
        {
            return string.Empty;
        }

        return Activator.CreateInstance(type);
    }

    public static bool CheckCompatibilityType(Type a, Type b)
    {
        if (a == typeof(float) || a == typeof(double))
        {
            return b == typeof(float) || b == typeof(double);
        }

        return a == b;
    }
}