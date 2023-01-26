using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using FlowGraphBase.Logger;
using FlowGraphBase.Script;

namespace FlowGraphBase
{
    public class GraphDataManager
    {
        public static GraphDataManager Instance { get; } = new GraphDataManager();

        public ObservableCollection<Sequence> GraphList { get; } = new ObservableCollection<Sequence>();

        public List<Sequence> GraphListBackup
        {
            get;
        }

        public ObservableCollection<SequenceFunction> GraphFunctionList { get; } = new ObservableCollection<SequenceFunction>();

        public List<SequenceFunction> GraphFunctionListBackup
        {
            get;
        }

        public ObservableCollection<ScriptElement> ScriptElementList { get; } = new ObservableCollection<ScriptElement>();

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

        public SequenceBase GetById(int id)
        {
            foreach (Sequence seq in GraphList)
            {
                if (seq.Id == id)
                {
                    return seq;
                }
            }

            foreach (SequenceFunction seq in GraphFunctionList)
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

            foreach (Sequence seq in GraphList)
            {
                if (string.Equals(seq.Name, name))
                {
                    return false;
                }
            }

            return true;
        }

        public Sequence GetSequenceById(int id)
        {
            foreach (Sequence seq in GraphList)
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

            foreach (SequenceFunction seq in GraphFunctionList)
            {
                if (string.Equals(seq.Name, name))
                {
                    return false;
                }
            }

            return true;
        }

        public SequenceFunction GetFunctionById(int id)
        {
            foreach (SequenceFunction seq in GraphFunctionList)
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

        public ScriptElement GetScriptById(int id)
        {
            foreach (ScriptElement el in ScriptElementList)
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

                foreach (XmlNode graphNode in node.SelectNodes("GraphList/Graph[@type='" + Sequence.XmlAttributeTypeValue + "']"))
                {
                    GraphList.Add(new Sequence(graphNode));
                }

                foreach (XmlNode graphNode in node.SelectNodes("GraphList/Graph[@type='" + SequenceFunction.XmlAttributeTypeValue + "']"))
                {
                    GraphFunctionList.Add(new SequenceFunction(graphNode));
                }

                foreach (XmlNode scriptNode in node.SelectNodes("ScriptList/Script"))
                {
                    ScriptElementList.Add(new ScriptElement(scriptNode));
                }

                //////////////////////////////////////////////////////////////////////////
                foreach (Sequence seq in GraphList)
                {
                    seq.ResolveNodesLinks(node.SelectSingleNode("GraphList/Graph[@id='" + seq.Id + "']"));
                }

                foreach (SequenceFunction seq in GraphFunctionList)
                {
                    seq.ResolveNodesLinks(node.SelectSingleNode("GraphList/Graph[@id='" + seq.Id + "']"));
                }
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteException(ex);
            }
        }

        public void Save(XmlNode node)
        {
            const int version = 1;

            XmlNode list = node.OwnerDocument.CreateElement("GraphList");
            node.AppendChild(list);

            list.AddAttribute("version", version.ToString());

            foreach (Sequence seq in GraphList)
            {
                seq.Save(list);
            }

            foreach (SequenceFunction seq in GraphFunctionList)
            {
                seq.Save(list);
            }

            XmlNode scriptList = node.OwnerDocument.CreateElement("ScriptList");
            node.AppendChild(scriptList);

            foreach (ScriptElement el in ScriptElementList)
            {
                el.Save(scriptList);
            }
        }
    }
}
