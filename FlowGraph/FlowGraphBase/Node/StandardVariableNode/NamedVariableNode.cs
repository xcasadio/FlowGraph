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
        NamedVariable _Value;

        public override string Title => _Value.Name;

        /// <summary>
        /// 
        /// </summary>
        public string VariableName => _Value.Name;

        /// <summary>
        /// 
        /// </summary>
        public Type VariableType => _Value.VariableType;

        /// <summary>
        /// 
        /// </summary>
        public override object Value
        {
            get => _Value.Value;
            set => _Value.InternalValueContainer.Value = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public NamedVariableNode(XmlNode node_)
            : base(node_)
        {
            InitializeSlots();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name_"></param>
        public NamedVariableNode(string name_)
        {
            _Value = NamedVariableManager.Instance.GetNamedVariable(name_);
            _Value.PropertyChanged += OnNamedVariablePropertyChanged;
            AddSlot(0, string.Empty, SlotType.VarInOut, _Value.VariableType);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitializeSlots()
        {
            base.InitializeSlots();

            if (_Value != null) // call only when loaded with xml
            {
                AddSlot(0, string.Empty, SlotType.VarInOut, _Value.VariableType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override SequenceNode CopyImpl()
        {
            return new NamedVariableNode(_Value.Name);
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
        protected override object LoadValue(XmlNode node_)
        {
            return NamedVariableManager.Instance.GetNamedVariable(node_.Attributes["varName"].Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        protected override void SaveValue(XmlNode node_)
        {
            node_.AddAttribute("varName", _Value.Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        protected override void Load(XmlNode node_)
        {
            base.Load(node_);
            _Value = (NamedVariable)LoadValue(node_.SelectSingleNode("Value"));
        }
    }
}
