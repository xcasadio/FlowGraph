using System;
using System.Collections.ObjectModel;
using System.Xml;

namespace FlowGraphBase
{
    public class NamedVariableManager
    {
        public static NamedVariableManager Instance { get; } = new NamedVariableManager();
        public event EventHandler<EventArgs<string, string>> OnRenamed;

        private const string NullToken = "<null>";

        public ObservableCollection<NamedVariable> Vars { get; } = new ObservableCollection<NamedVariable>();

        private NamedVariableManager()
        { }

        internal NamedVariable GetNamedVariable(string name)
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

        internal ValueContainer GetValueContainer(string name)
        {
            ValueContainer container = null;

            foreach (var v in Vars)
            {
                if (string.Equals(v.Name, name))
                {
                    container = v.InternalValueContainer;
                    break;
                }
            }

            return container;
        }

        public bool TryGet(string name, out object var)
        {
            bool res = false;
            var = null;

            foreach (var v in Vars)
            {
                if (string.Equals(v.Name, name))
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
            NamedVariable namedVar = new NamedVariable(name, new ValueContainer(var?.GetType(), var));
            Vars.Add(namedVar);
        }

        private void Add(string name, ValueContainer var)
        {
            NamedVariable namedVar = new NamedVariable(name, var);
            Vars.Add(namedVar);
        }

        public void Remove(NamedVariable var)
        {
            Vars.Remove(var);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name_"></param>
        //         public void Remove(string name_)
        //         {
        //             foreach (var v in _Vars)
        //             {
        //                 if (string.Equals(v.Name, name_) == true)
        //                 {
        //                     _Vars.Remove(v);
        //                     break;
        //                 }
        //             }
        //         }

        public void Rename(string name, string newName)
        {
            foreach (var v in Vars)
            {
                if (string.Equals(v.Name, name))
                {
                    v.Name = newName;

                    OnRenamed?.Invoke(this, new EventArgs<string, string>(name, newName));

                    break;
                }
            }
        }

        public void Update(string name, object var)
        {
            foreach (var v in Vars)
            {
                if (string.Equals(v.Name, name))
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
                if (string.Equals(v.Name, name))
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

        public void Load(XmlNode node)
        {
            XmlNode varListNode = node.SelectSingleNode("NamedVariableList");
            int version = int.Parse(varListNode.Attributes["version"].Value);

            Clear();

            foreach (XmlNode namedVarNode in varListNode.SelectNodes("NamedVariable"))
            {
                int varVersion = int.Parse(namedVarNode.Attributes["version"].Value);
                string key = namedVarNode.Attributes["key"].Value;
                ValueContainer value = new ValueContainer(namedVarNode);
                Add(key, value);
            }
        }

        public void Save(XmlNode node)
        {
            const int version = 1;
            const int varVersion = 1;

            XmlNode list = node.OwnerDocument.CreateElement("NamedVariableList");
            node.AppendChild(list);
            list.AddAttribute("version", version.ToString());

            foreach (var v in Vars)
            {
                XmlNode varNode = node.OwnerDocument.CreateElement("NamedVariable");
                list.AppendChild(varNode);
                varNode.AddAttribute("version", varVersion.ToString());
                varNode.AddAttribute("key", v.Name);
                v.InternalValueContainer.Save(varNode);
            }
        }
    }
}
