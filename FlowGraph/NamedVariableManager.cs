using System.Collections.ObjectModel;
using System.Xml;

namespace FlowGraph
{
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

        public void Load(XmlNode node)
        {
            var varListNode = node.SelectSingleNode("NamedVariableList");
            var version = int.Parse(varListNode?.Attributes?["version"]?.Value ?? string.Empty);

            Clear();

            foreach (XmlNode namedVarNode in varListNode?.SelectNodes("NamedVariable")!)
            {
                var varVersion = int.Parse(namedVarNode.Attributes["version"].Value);
                var key = namedVarNode.Attributes["key"].Value;
                var value = new ValueContainer(namedVarNode);
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
