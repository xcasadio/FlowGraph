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

    public static void Register(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes()
                     .Where(t => t is { IsClass: true, IsGenericType: false, IsInterface: false, IsAbstract: false }))
        {
            if (!type.IsSubclassOf(typeof(SequenceNode)))
            {
                continue;
            }

            var browsableAttribute = Attribute.GetCustomAttribute(type, typeof(Visible), true);
            if (browsableAttribute != null && ((Visible)browsableAttribute).Value == false)
            {
                continue;
            }

            var customAttribute = Attribute.GetCustomAttribute(type, typeof(Category));
            var nameAtt = Attribute.GetCustomAttribute(type, typeof(Name));

            var displayName = ((Name)nameAtt)?.DisplayName;
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