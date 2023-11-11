using FlowGraph.Logger;
using Logger;

namespace FlowGraph.Script;

public static class GlobalVar
{
    private static readonly Dictionary<string?, object> Vars = new();

    public static void Set(string? key, object val)
    {
        if (Vars.ContainsKey(key))
        {
            Vars[key] = val;
        }
        else
        {
            Vars.Add(key, val);
        }
    }

    public static object Get(string? key)
    {
        if (Vars.ContainsKey(key) == false)
        {
            LogManager.Instance.WriteLine(LogVerbosity.Error, "GlobalVar.Get() : can't find the field with the key '{0}'", key);
            return null;
        }

        return Vars[key];
    }

    public static bool ContainsKey(string? key)
    {
        return Vars.ContainsKey(key);
    }
}

public static class ScriptSequence
{
    private static volatile uint _sequence;


    public static uint GetNext()
    {
        return _sequence++;
    }
}