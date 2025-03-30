using System.Reflection;
using FlowGraph.Attributes;
using FlowGraph.Logger;
using FlowGraph.Nodes;
using Logger;

namespace FlowGraph;

public static class NodeRegister
{
    public static SortedDictionary<string, List<(string name, Type type)>> NodeTypesByCategory { get; } = new();
    public static List<Type> NodeTypes { get; } = new();

    public static void RegisterAssemblies(IEnumerable<Assembly> assemblies)
    {
        foreach (var assembly in assemblies)
        {
            Register(assembly);
        }
    }

    public static void Register(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes()
                     .Where(t => t is { IsClass: true, IsGenericType: false, IsInterface: false, IsAbstract: false }
                                    && t.IsSubclassOf(typeof(SequenceNode))))
        {
            var browsableAttribute = Attribute.GetCustomAttribute(type, typeof(Visible), true);
            if (browsableAttribute != null && ((Visible)browsableAttribute).Value == false)
            {
                continue;
            }

            var customAttribute = Attribute.GetCustomAttribute(type, typeof(Category));
            var nameAtt = Attribute.GetCustomAttribute(type, typeof(Name));

            var displayName = (nameAtt as Name)?.DisplayName;
            if (nameAtt == null
                || string.IsNullOrWhiteSpace(displayName))
            {
                LogManager.Instance.WriteLine(
                    LogVerbosity.Error,
                    "Can't create menu for the type '{0}' because the attribute Name is not specified",
                    type.FullName);
                continue;
            }

            NodeTypes.Add(type);

            var categoryPath = customAttribute == null ? string.Empty : ((Category)customAttribute).CategoryPath;

            if (!NodeTypesByCategory.TryGetValue(categoryPath, out var nodeDescription))
            {
                nodeDescription = new();
                NodeTypesByCategory.Add(categoryPath, nodeDescription);
            }

            nodeDescription.Add(new(displayName, type));
        }
    }

    public static void Clear()
    {
        NodeTypesByCategory.Clear();
    }
}