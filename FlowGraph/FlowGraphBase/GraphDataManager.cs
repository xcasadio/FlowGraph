using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using FlowGraphBase.Logger;
using FlowGraphBase.Script;

namespace FlowGraphBase
{
    /// <summary>
    /// 
    /// </summary>
    public class GraphDataManager
    {
        #region Singleton

        /// <summary>
        /// Gets
        /// </summary>
        public static GraphDataManager Instance { get; } = new GraphDataManager();

        #endregion //Singleton

        #region Fields

        #endregion //Fields

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<Sequence> GraphList { get; } = new ObservableCollection<Sequence>();

        /// <summary>
        /// Gets/Sets
        /// </summary>
        public List<Sequence> GraphListBackup
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<SequenceFunction> GraphFunctionList { get; } = new ObservableCollection<SequenceFunction>();

        /// <summary>
        /// Gets/Sets
        /// </summary>
        public List<SequenceFunction> GraphFunctionListBackup
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<ScriptElement> ScriptElementList { get; } = new ObservableCollection<ScriptElement>();

        /// <summary>
        /// Gets/Sets
        /// </summary>
        public List<ScriptElement> ScriptElementListBackup
        {
            get;
            private set;
        }

        #endregion //Properties

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        private GraphDataManager()
        {
            GraphListBackup = new List<Sequence>();
            GraphFunctionListBackup = new List<SequenceFunction>();
            ScriptElementListBackup = new List<ScriptElement>();
        }

        #endregion //Constructors

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            GraphList.Clear();
            GraphFunctionList.Clear();
            ScriptElementList.Clear();

            GraphListBackup.Clear();
            GraphFunctionListBackup.Clear();
            ScriptElementListBackup.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_"></param>
        /// <returns></returns>
        public SequenceBase GetByID(int id_)
        {
            foreach (SequenceBase seq_ in GraphList)
            {
                if (seq_.Id == id_)
                {
                    return seq_;
                }
            }

            foreach (SequenceBase seq_ in GraphFunctionList)
            {
                if (seq_.Id == id_)
                {
                    return seq_;
                }
            }

            return null;
        }

        #region Sequence

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name_"></param>
        /// <returns></returns>
        public bool IsValidSequenceName(string name_)
        {
            if (string.IsNullOrWhiteSpace(name_))
            {
                return false;
            }

            foreach (Sequence seq in GraphList)
            {
                if (string.Equals(seq.Name, name_))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_"></param>
        /// <returns></returns>
        public Sequence GetSequenceByID(int id_)
        {
            foreach (Sequence seq_ in GraphList)
            {
                if (seq_.Id == id_)
                {
                    return seq_;
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearSequences()
        {
            GraphList.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddSequence(Sequence seq_)
        {
            GraphList.Add(seq_);
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveSequence(Sequence seq_)
        {
            GraphList.Remove(seq_);
        }

        #endregion // Sequence

        #region Function

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name_"></param>
        /// <returns></returns>
        public bool IsValidFunctionName(string name_)
        {
            if (string.IsNullOrWhiteSpace(name_))
            {
                return false;
            }

            foreach (SequenceFunction seq in GraphFunctionList)
            {
                if (string.Equals(seq.Name, name_))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_"></param>
        /// <returns></returns>
        public SequenceFunction GetFunctionByID(int id_)
        {
            foreach (SequenceFunction seq_ in GraphFunctionList)
            {
                if (seq_.Id == id_)
                {
                    return seq_;
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearFunctionList()
        {
            GraphFunctionList.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddFunction(SequenceFunction seq_)
        {
            GraphFunctionList.Add(seq_);
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveFunction(SequenceFunction seq_)
        {
            GraphFunctionList.Remove(seq_);
        }

        #endregion // Sequence

        #region Script

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_"></param>
        /// <returns></returns>
        public ScriptElement GetScriptByID(int id_)
        {
            foreach (ScriptElement el in ScriptElementList)
            {
                if (el.ID == id_)
                {
                    return el;
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearScripts()
        {
            ScriptElementList.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="el_"></param>
        public void AddScript(ScriptElement el_)
        {
            ScriptElementList.Add(el_);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="el_"></param>
        public void RemoveScript(ScriptElement el_)
        {
            ScriptElementList.Remove(el_);
        }

        #endregion

        #region Persistence

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public void Load(XmlNode node_)
        {
            try
            {
                int version = int.Parse(node_.SelectSingleNode("GraphList").Attributes["version"].Value);

                foreach (XmlNode graphNode in node_.SelectNodes("GraphList/Graph[@type='" + Sequence.XmlAttributeTypeValue + "']"))
                {
                    GraphList.Add(new Sequence(graphNode));
                }

                foreach (XmlNode graphNode in node_.SelectNodes("GraphList/Graph[@type='" + SequenceFunction.XmlAttributeTypeValue + "']"))
                {
                    GraphFunctionList.Add(new SequenceFunction(graphNode));
                }

                foreach (XmlNode scriptNode in node_.SelectNodes("ScriptList/Script"))
                {
                    ScriptElementList.Add(new ScriptElement(scriptNode));
                }

                //////////////////////////////////////////////////////////////////////////
                foreach (Sequence seq in GraphList)
                {
                    seq.ResolveNodesLinks(node_.SelectSingleNode("GraphList/Graph[@id='" + seq.Id + "']"));
                }

                foreach (SequenceFunction seq in GraphFunctionList)
                {
                    seq.ResolveNodesLinks(node_.SelectSingleNode("GraphList/Graph[@id='" + seq.Id + "']"));
                }
            }
            catch (System.Exception ex)
            {
                LogManager.Instance.WriteException(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public void Save(XmlNode node_)
        {
            const int version = 1;

            XmlNode list = node_.OwnerDocument.CreateElement("GraphList");
            node_.AppendChild(list);

            list.AddAttribute("version", version.ToString());

            foreach (Sequence seq in GraphList)
            {
                seq.Save(list);
            }

            foreach (SequenceFunction seq in GraphFunctionList)
            {
                seq.Save(list);
            }

            XmlNode scriptList = node_.OwnerDocument.CreateElement("ScriptList");
            node_.AppendChild(scriptList);

            foreach (ScriptElement el in ScriptElementList)
            {
                el.Save(scriptList);
            }
        }

        #endregion // Persistence

        #endregion //Methods
    }
}
