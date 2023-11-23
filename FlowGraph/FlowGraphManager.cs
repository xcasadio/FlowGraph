﻿using System.Diagnostics;
using System.Xml;
using FlowGraph.Logger;
using Logger;

namespace FlowGraph;

public class FlowGraphManager
{
    public Sequence Sequence { get; } = new("sequence");
    public List<SequenceFunction> Functions { get; } = new();

    //macros ?
    //variables

    public void Load(XmlNode node)
    {
        try
        {
            //int version = int.Parse(node.SelectSingleNode("GraphList").Attributes["version"].Value);

            var graphNodes = node.SelectNodes("GraphList/Graph[@type='" + Sequence.XmlAttributeTypeValue + "']");
            if (graphNodes != null)
            {
                foreach (XmlNode graphNode in graphNodes)
                {
                    Debugger.Break(); // only one sequence
                    Sequence.Load(graphNode);
                }
            }

            var functionNodes = node.SelectNodes("GraphList/Graph[@type='" + SequenceFunction.XmlAttributeTypeValue + "']");
            if (functionNodes != null)
            {
                foreach (XmlNode graphNode in functionNodes)
                {
                    Debugger.Break();
                    //Functions.Add(new SequenceFunction(graphNode));
                }
            }

            ResolveLinks(node, Sequence);

            foreach (var function in Functions)
            {
                ResolveLinks(node, function);
            }
        }
        catch (Exception ex)
        {
            LogManager.Instance.WriteException(ex);
        }
    }

    private static void ResolveLinks(XmlNode node, SequenceBase seq)
    {
        var graphNode = node.SelectSingleNode("GraphList/Graph[@id='" + seq.Id + "']");

        if (graphNode != null)
        {
            seq.ResolveNodesLinks(graphNode);
        }
        else
        {
            LogManager.Instance.WriteLine(LogVerbosity.Error, $"Can't find the graph {seq.Id}");
        }
    }

    public void Save(XmlNode node)
    {
        const int version = 1;

        XmlNode list = node.OwnerDocument!.CreateElement("GraphList");
        node.AppendChild(list);

        list.AddAttribute("version", version.ToString());

        Sequence.Save(list);

        foreach (var seq in Functions)
        {
            seq.Save(list);
        }
    }
}