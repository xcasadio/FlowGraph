using System;
using System.Xml;
using System.ComponentModel;

namespace FlowGraphBase.Node.StandardVariableNode
{
    /// <summary>
    /// 
    /// </summary>
    [Visible(false)]
    public class NamedVariableNode : VariableNode
    {
		#region Fields

        NamedVariable m_Value;

		#endregion //Fields
	
		#region Properties

        public override string Title => m_Value.Name;

        /// <summary>
        /// 
        /// </summary>
        public string VariableName => m_Value.Name;

        /// <summary>
        /// 
        /// </summary>
        public Type VariableType => m_Value.VariableType;

        /// <summary>
        /// 
        /// </summary>
        public override object Value
        {
            get => m_Value.Value;
            set => m_Value.InternalValueContainer.Value = value;
        }

		#endregion //Properties
	
		#region Constructors
		
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
            : base()
        {
            m_Value = NamedVariableManager.Instance.GetNamedVariable(name_);
            m_Value.PropertyChanged += new PropertyChangedEventHandler(OnNamedVariablePropertyChanged);
            AddSlot(0, string.Empty, SlotType.VarInOut, m_Value.VariableType, true, VariableControlType.ReadOnly);
        }

		#endregion //Constructors
	
		#region Methods

        /// <summary>
        /// 
        /// </summary>
        protected override void InitializeSlots()
        {
            base.InitializeSlots();

            if (m_Value != null) // call only when loaded with xml
            {
                AddSlot(0, string.Empty, SlotType.VarInOut, m_Value.VariableType, true, VariableControlType.ReadOnly);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override SequenceNode CopyImpl()
        {
            return new NamedVariableNode(m_Value.Name);
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
            node_.AddAttribute("varName", m_Value.Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        protected override void Load(XmlNode node_)
        {
            base.Load(node_);
            m_Value = (NamedVariable)LoadValue(node_.SelectSingleNode("Value"));
        }
		#endregion //Methods
    }
}
