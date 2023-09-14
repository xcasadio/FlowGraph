using System.Collections.ObjectModel;
using System.Xml;
using FlowGraph.Logger;
using FlowGraph.Script;

namespace FlowGraph;

public class GraphDataManager
{
    public static GraphDataManager Instance { get; } = new();

    public ObservableCollection<Sequence> GraphList { get; } = new();

    public List<Sequence> GraphListBackup
    {
        get;
    }

    public ObservableCollection<SequenceFunction> GraphFunctionList { get; } = new();

    public List<SequenceFunction> GraphFunctionListBackup
    {
        get;
    }

    public ObservableCollection<ScriptElement> ScriptElementList { get; } = new();

    public List<ScriptElement> ScriptElementListBackup
    {
        get;
    }

    private GraphDataManager()
    {
        GraphListBackup = new List<Sequence>();
        GraphFunctionListBackup = new List<SequenceFunction>();
        ScriptElementListBackup = new List<ScriptElement>();
    }

    public void Clear()
    {
        GraphList.Clear();
        GraphFunctionList.Clear();
        ScriptElementList.Clear();

        GraphListBackup.Clear();
        GraphFunctionListBackup.Clear();
        ScriptElementListBackup.Clear();
    }

    public bool IsChanges()
    {
        // script
        if (ScriptElementList.Count != ScriptElementListBackup.Count)
        {
            return true;
        }

        //             for (int i=0; i<_ScriptElementList.Count)
        //             {
        //                 if (string.Equals(
        //                         ScriptElementListBackup[i].ScriptSourceCode,
        //                         _ScriptElementList[i].ScriptSourceCode) == false)
        //                 {
        //                     return true;
        //                 }
        //             }

        return false;
    }

    public SequenceBase? GetById(int id)
    {
        foreach (var seq in GraphList)
        {
            if (seq.Id == id)
            {
                return seq;
            }
        }

        foreach (var seq in GraphFunctionList)
        {
            if (seq.Id == id)
            {
                return seq;
            }
        }

        return null;
    }

    public bool IsValidSequenceName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        foreach (var seq in GraphList)
        {
            if (string.Equals(seq.Name, name))
            {
                return false;
            }
        }

        return true;
    }

    public Sequence? GetSequenceById(int id)
    {
        foreach (var seq in GraphList)
        {
            if (seq.Id == id)
            {
                return seq;
            }
        }

        return null;
    }

    public void ClearSequences()
    {
        GraphList.Clear();
    }

    public void AddSequence(Sequence seq)
    {
        GraphList.Add(seq);
    }

    public void RemoveSequence(Sequence seq)
    {
        GraphList.Remove(seq);
    }

    public bool IsValidFunctionName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        foreach (var seq in GraphFunctionList)
        {
            if (string.Equals(seq.Name, name))
            {
                return false;
            }
        }

        return true;
    }

    public SequenceFunction? GetFunctionById(int id)
    {
        foreach (var seq in GraphFunctionList)
        {
            if (seq.Id == id)
            {
                return seq;
            }
        }

        return null;
    }

    public void ClearFunctionList()
    {
        GraphFunctionList.Clear();
    }

    public void AddFunction(SequenceFunction seq)
    {
        GraphFunctionList.Add(seq);
    }

    public void RemoveFunction(SequenceFunction seq)
    {
        GraphFunctionList.Remove(seq);
    }

    public ScriptElement? GetScriptById(int id)
    {
        foreach (var el in ScriptElementList)
        {
            if (el.Id == id)
            {
                return el;
            }
        }

        return null;
    }

    public void ClearScripts()
    {
        ScriptElementList.Clear();
    }

    public void AddScript(ScriptElement el)
    {
        ScriptElementList.Add(el);
    }

    public void RemoveScript(ScriptElement el)
    {
        ScriptElementList.Remove(el);
    }

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
                    GraphList.Add(new Sequence(graphNode));
                }
            }

            var functionNodes = node.SelectNodes("GraphList/Graph[@type='" + SequenceFunction.XmlAttributeTypeValue + "']");
            if (functionNodes != null)
            {
                foreach (XmlNode graphNode in functionNodes)
                {
                    GraphFunctionList.Add(new SequenceFunction(graphNode));
                }
            }

            var elementNodes = node.SelectNodes("ScriptList/Script");
            if (elementNodes != null)
            {
                foreach (XmlNode scriptNode in elementNodes)
                {
                    ScriptElementList.Add(new ScriptElement(scriptNode));
                }
            }

            foreach (var seq in GraphList)
            {
                ResolveLinks(node, seq);
            }

            foreach (var seq in GraphFunctionList)
            {
                ResolveLinks(node, seq);
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

        foreach (var seq in GraphList)
        {
            seq.Save(list);
        }

        foreach (var seq in GraphFunctionList)
        {
            seq.Save(list);
        }

        XmlNode scriptList = node.OwnerDocument.CreateElement("ScriptList");
        node.AppendChild(scriptList);

        foreach (var el in ScriptElementList)
        {
            el.Save(scriptList);
        }
    }
}