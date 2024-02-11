using System.Collections.ObjectModel;
using Newtonsoft.Json.Linq;
using System.Xml;

namespace FlowGraph;

public class NamedVariableManager
{
    public static NamedVariableManager Instance { get; } = new();
    public event EventHandler<EventArgs<string, string?>> OnRenamed;

    private const string NullToken = "<null>";

    public ObservableCollection<NamedVariable> Vars { get; } = new();

    private NamedVariableManager()
    { }

    internal NamedVariable? GetNamedVariable(string? name)
    {
        foreach (var v in Vars)
        {
            if (string.Equals(v.Name, name))
            {
                return v;
            }
        }

        return null;
    }

    internal ValueContainer? GetValueContainer(string name)
    {
        return Vars.SingleOrDefault(x => string.Equals(x.Name, name, StringComparison.InvariantCultureIgnoreCase))?.InternalValueContainer;
    }

    public bool TryGet(string name, out object? var)
    {
        var res = false;
        var = null;

        foreach (var v in Vars)
        {
            if (string.Equals(v.Name, name, StringComparison.InvariantCultureIgnoreCase))
            {
                var = v.Value;
                res = true;
                break;
            }
        }

        return res;
    }

    public void Add(string name, object var)
    {
        var namedVar = new NamedVariable(name, new ValueContainer(var.GetType(), var));
        Vars.Add(namedVar);
    }

    private void Add(string? name, ValueContainer var)
    {
        var namedVar = new NamedVariable(name, var);
        Vars.Add(namedVar);
    }

    public void Remove(NamedVariable var)
    {
        Vars.Remove(var);
    }

    public void Rename(string name, string newName)
    {
        foreach (var v in Vars)
        {
            if (string.Equals(v.Name, name, StringComparison.InvariantCultureIgnoreCase))
            {
                v.Name = newName;

                OnRenamed(this, new EventArgs<string, string?>(name, newName));

                break;
            }
        }
    }

    public void Update(string name, object? var)
    {
        foreach (var v in Vars)
        {
            if (string.Equals(v.Name, name, StringComparison.InvariantCultureIgnoreCase))
            {
                v.InternalValueContainer.Value = var;
                break;
            }
        }
    }

    public bool Contains(string name)
    {
        foreach (var v in Vars)
        {
            if (string.Equals(v.Name, name, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    public void Clear()
    {
        Vars.Clear();
    }

    public bool IsValidName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        return !Contains(name);
    }

    public void Load(JObject node)
    {
        Clear();

        System.Diagnostics.Debugger.Break();

        foreach (var namedVarNode in node["named_variables"])
        {
            var key = namedVarNode["key"].Value<string>();
            var value = new ValueContainer();
            //value.Load(namedVarNode);
            Add(key, value);
        }
    }

    public void Save(JObject node)
    {
        var variableNodeArray = new JArray();

        foreach (var v in Vars)
        {
            var varNode = new JObject();
            varNode["key"] = v.Name;
            v.InternalValueContainer.Save(varNode);
            variableNodeArray.Add(varNode);
        }

        node["named_variables"] = variableNodeArray;
    }
}