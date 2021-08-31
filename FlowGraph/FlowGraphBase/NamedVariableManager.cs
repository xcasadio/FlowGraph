using System;
using System.Xml;
using System.Collections.ObjectModel;

namespace FlowGraphBase
{
    /// <summary>
    /// 
    /// </summary>
    public class NamedVariableManager
    {
        #region Singleton

        static private NamedVariableManager m_Instance = new NamedVariableManager();

        /// <summary>
        /// Gets
        /// </summary>
        static public NamedVariableManager Instance => m_Instance;

        #endregion //Singleton

		#region Fields

        public event EventHandler<EventArgs<string, string>> OnRenamed;

        private const string NullToken = "<null>";
        private ObservableCollection<NamedVariable> m_Vars = new ObservableCollection<NamedVariable>();

		#endregion //Fields
	
		#region Properties

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<NamedVariable> Vars => m_Vars;

        #endregion //Properties
	
		#region Constructors

        /// <summary>
        /// 
        /// </summary>
        private NamedVariableManager()
        { }

		#endregion //Constructors
	
		#region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name_"></param>
        /// <returns></returns>
        internal NamedVariable GetNamedVariable(string name_)
        {
            foreach (var v in m_Vars)
            {
                if (string.Equals(v.Name, name_))
                {
                    return v;
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name_"></param>
        /// <returns></returns>
        internal ValueContainer GetValueContainer(string name_)
        {
            ValueContainer container = null;

            foreach (var v in m_Vars)
            {
                if (string.Equals(v.Name, name_))
                {
                    container = v.InternalValueContainer;
                    break;
                }
            }

            return container;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name_"></param>
        /// <param name="var_"></param>
        /// <returns></returns>
        public bool TryGet(string name_, out object var_)
        {
            bool res = false;
            var_ = null;

            foreach (var v in m_Vars)
            {
                if (string.Equals(v.Name, name_))
                {
                    var_ = v.Value;
                    res = true;
                    break;
                }
            }

            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name_"></param>
        /// <param name="var_"></param>
        public void Add(string name_, object var_)
        {
            NamedVariable namedVar = new NamedVariable(name_, new ValueContainer(var_ == null ? null : var_.GetType(), var_));
            m_Vars.Add(namedVar);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name_"></param>
        /// <param name="var_"></param>
        private void Add(string name_, ValueContainer var_)
        {
            NamedVariable namedVar = new NamedVariable(name_, var_);
            m_Vars.Add(namedVar);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name_"></param>
        public void Remove(NamedVariable var_)
        {
            m_Vars.Remove(var_);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name_"></param>
//         public void Remove(string name_)
//         {
//             foreach (var v in m_Vars)
//             {
//                 if (string.Equals(v.Name, name_) == true)
//                 {
//                     m_Vars.Remove(v);
//                     break;
//                 }
//             }
//         }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name_"></param>
        /// <param name="newName_"></param>
        public void Rename(string name_, string newName_)
        {
            foreach (var v in m_Vars)
            {
                if (string.Equals(v.Name, name_))
                {
                    v.Name = newName_;

                    if (OnRenamed != null)
                    {
                        OnRenamed(this, new EventArgs<string, string>(name_, newName_));
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name_"></param>
        /// <param name="var_"></param>
        public void Update(string name_, object var_)
        {
            foreach (var v in m_Vars)
            {
                if (string.Equals(v.Name, name_))
                {
                    v.InternalValueContainer.Value = var_;
                    break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name_"></param>
        /// <returns></returns>
        public bool Contains(string name_)
        {
            foreach (var v in m_Vars)
            {
                if (string.Equals(v.Name, name_))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            m_Vars.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name_"></param>
        /// <returns></returns>
        public bool IsValidName(string name_)
        {
            if (string.IsNullOrWhiteSpace(name_))
            {
                return false;
            }

            return !Contains(name_);
        }

        #region Persistence

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public void Load(XmlNode node_)
        {
            XmlNode varListNode = node_.SelectSingleNode("NamedVariableList");
            int version = int.Parse(varListNode.Attributes["version"].Value);

            Clear();

            foreach (XmlNode node in varListNode.SelectNodes("NamedVariable"))
            {
                int varVersion = int.Parse(node.Attributes["version"].Value);
                string key = node.Attributes["key"].Value;
                ValueContainer value = new ValueContainer(node);
                Add(key, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public void Save(XmlNode node_)
        {
            const int version = 1;
            const int varVersion = 1;

            XmlNode list = node_.OwnerDocument.CreateElement("NamedVariableList");
            node_.AppendChild(list);
            list.AddAttribute("version", version.ToString());

            foreach (var v in m_Vars)
            {
                XmlNode varNode = node_.OwnerDocument.CreateElement("NamedVariable");
                list.AppendChild(varNode);
                varNode.AddAttribute("version", varVersion.ToString());
                varNode.AddAttribute("key", v.Name);
                v.InternalValueContainer.Save(varNode);
            }
        }

        #endregion // Persistence

        #endregion //Methods
    }
}
