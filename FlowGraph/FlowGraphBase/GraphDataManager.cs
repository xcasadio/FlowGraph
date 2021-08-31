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

        static private GraphDataManager m_Instance = new GraphDataManager();

        /// <summary>
        /// Gets
        /// </summary>
        static public GraphDataManager Instance => m_Instance;

        #endregion //Singleton

        #region Fields

        private ObservableCollection<Sequence> m_GraphList = new ObservableCollection<Sequence>();
        private ObservableCollection<SequenceFunction> m_GraphFunctionList = new ObservableCollection<SequenceFunction>();
        private ObservableCollection<ScriptElement> m_ScriptElementList = new ObservableCollection<ScriptElement>();

        #endregion //Fields

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<Sequence> GraphList => m_GraphList;

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
        public ObservableCollection<SequenceFunction> GraphFunctionList => m_GraphFunctionList;

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
        public ObservableCollection<ScriptElement> ScriptElementList => m_ScriptElementList;

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
            m_GraphList.Clear();
            m_GraphFunctionList.Clear();
            m_ScriptElementList.Clear();

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
            if (m_ScriptElementList.Count != ScriptElementListBackup.Count)
            {
                return true;
            }

//             for (int i=0; i<m_ScriptElementList.Count)
//             {
//                 if (string.Equals(
//                         ScriptElementListBackup[i].ScriptSourceCode,
//                         m_ScriptElementList[i].ScriptSourceCode) == false)
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
            foreach (SequenceBase seq_ in m_GraphList)
            {
                if (seq_.Id == id_)
                {
                    return seq_;
                }
            }

            foreach (SequenceBase seq_ in m_GraphFunctionList)
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

            foreach (Sequence seq in m_GraphList)
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
            foreach (Sequence seq_ in m_GraphList)
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
            m_GraphList.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddSequence(Sequence seq_)
        {
            m_GraphList.Add(seq_);
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveSequence(Sequence seq_)
        {
            m_GraphList.Remove(seq_);
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

            foreach (SequenceFunction seq in m_GraphFunctionList)
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
            foreach (SequenceFunction seq_ in m_GraphFunctionList)
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
            m_GraphFunctionList.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddFunction(SequenceFunction seq_)
        {
            m_GraphFunctionList.Add(seq_);
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveFunction(SequenceFunction seq_)
        {
            m_GraphFunctionList.Remove(seq_);
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
            foreach (ScriptElement el in m_ScriptElementList)
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
            m_ScriptElementList.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="el_"></param>
        public void AddScript(ScriptElement el_)
        {
            m_ScriptElementList.Add(el_);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="el_"></param>
        public void RemoveScript(ScriptElement el_)
        {
            m_ScriptElementList.Remove(el_);
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
                    m_GraphList.Add(new Sequence(graphNode));
                }

                foreach (XmlNode graphNode in node_.SelectNodes("GraphList/Graph[@type='" + SequenceFunction.XmlAttributeTypeValue + "']"))
                {
                    m_GraphFunctionList.Add(new SequenceFunction(graphNode));
                }

                foreach (XmlNode scriptNode in node_.SelectNodes("ScriptList/Script"))
                {
                    m_ScriptElementList.Add(new ScriptElement(scriptNode));
                }

                //////////////////////////////////////////////////////////////////////////
                foreach (Sequence seq in m_GraphList)
                {
                    seq.ResolveNodesLinks(node_.SelectSingleNode("GraphList/Graph[@id='" + seq.Id + "']"));
                }

                foreach (SequenceFunction seq in m_GraphFunctionList)
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

            foreach (Sequence seq in m_GraphList)
            {
                seq.Save(list);
            }

            foreach (SequenceFunction seq in m_GraphFunctionList)
            {
                seq.Save(list);
            }

            XmlNode scriptList = node_.OwnerDocument.CreateElement("ScriptList");
            node_.AppendChild(scriptList);

            foreach (ScriptElement el in m_ScriptElementList)
            {
                el.Save(scriptList);
            }
        }

        #endregion // Persistence

        #endregion //Methods
    }
}
