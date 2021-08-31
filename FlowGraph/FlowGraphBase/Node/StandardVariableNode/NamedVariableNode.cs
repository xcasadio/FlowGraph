using System;
using System.ComponentModel;
using System.Xml;

namespace FlowGraphBase.Node.StandardVariableNode
{
    /// <summary>
    /// 
    /// </summary>
    [Visible(false)]
    public class NamedVariableNode : VariableNode
    {
        NamedVariable _value;

        public override string Title => _value.Name;

        /// <summary>
        /// 
        /// </summary>
        public string VariableName => _value.Name;

        /// <summary>
        /// 
        /// </summary>
        public Type VariableType => _value.VariableType;

        /// <summary>
        /// 
        /// </summary>
        public override object Value
        {
            get => _value.Value;
            set => _value.InternalValueContainer.Value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public NamedVariableNode(XmlNode node)
            : base(node)
        {
            InitializeSlots();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name_"></param>
        public NamedVariableNode(string name)
        {
            _value = NamedVariableManager.Instance.GetNamedVariable(name);
            _value.PropertyChanged += OnNamedVariablePropertyChanged;
            AddSlot(0, string.Empty, SlotType.VarInOut, _value.VariableType);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitializeSlots()
        {
            base.InitializeSlots();

            if (_value != null) // call only when loaded with xml
            {
                AddSlot(0, string.Empty, SlotType.VarInOut, _value.VariableType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override SequenceNode CopyImpl()
        {
            return new NamedVariableNode(_value.Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNamedVariablePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        /// <returns></returns>
        protected override object LoadValue(XmlNode node)
        {
            return NamedVariableManager.Instance.GetNamedVariable(node.Attributes["varName"].Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        protected override void SaveValue(XmlNode node)
        {
            node.AddAttribute("varName", _value.Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        protected override void Load(XmlNode node)
        {
            base.Load(node);
            _value = (NamedVariable)LoadValue(node.SelectSingleNode("Value"));
        }
    }
}
