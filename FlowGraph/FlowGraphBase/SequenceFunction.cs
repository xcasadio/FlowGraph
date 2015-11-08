using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using FlowGraphBase.Node.StandardEventNode;
using FlowGraphBase.Node.StandardActionNode;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace FlowGraphBase
{
    /// <summary>
    /// 
    /// </summary>
    public class SequenceFunction : SequenceBase
    {
		#region Fields

        public const string XmlAttributeTypeValue = "Function";

        public event EventHandler<FunctionSlotChangedEventArg> FunctionSlotChanged;

        private ObservableCollection<SequenceFunctionSlot> m_Slots = new ObservableCollection<SequenceFunctionSlot>();
        private int m_NextSlotId = 0;

		#endregion //Fields

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<SequenceFunctionSlot> Inputs
        {
            get 
            {
                foreach (SequenceFunctionSlot s in m_Slots)
                {
                    if (s.SlotType == FunctionSlotType.Input)
                    {
                        yield return s;
                    }
                }                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<SequenceFunctionSlot> Outputs
        {
            get
            {
                foreach (SequenceFunctionSlot s in m_Slots)
                {
                    if (s.SlotType == FunctionSlotType.Output)
                    {
                        yield return s;
                    }
                }
            }
        }

		#endregion //Properties
	
		#region Constructors
		
        /// <summary>
        /// 
        /// </summary>
        public SequenceFunction(string name_)
            : base(name_)
        {
            AddNode(new OnEnterFunctionEvent(this));
            AddNode(new ReturnNode(this));

            m_Slots.CollectionChanged += new NotifyCollectionChangedEventHandler(OnSlotCollectionChanged);
        }

        /// <summary>
        /// 
        /// </summary>
        public SequenceFunction(XmlNode node_)
            : base(node_)
        {
            m_Slots.CollectionChanged += new NotifyCollectionChangedEventHandler(OnSlotCollectionChanged);
        }

		#endregion //Constructors
	
		#region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name_"></param>
        public void AddInput(string name_)
        {
            AddSlot(new SequenceFunctionSlot(++m_NextSlotId, FunctionSlotType.Input) { Name = name_ });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name_"></param>
        public void AddOutput(string name_)
        {
            AddSlot(new SequenceFunctionSlot(++m_NextSlotId, FunctionSlotType.Output) { Name = name_ });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slot_"></param>
        private void AddSlot(SequenceFunctionSlot slot_)
        {
            slot_.IsArray = false;
            slot_.VariableType = typeof(int);

            m_Slots.Add(slot_);

            if (FunctionSlotChanged != null)
            {
                FunctionSlotChanged(this, new FunctionSlotChangedEventArg(FunctionSlotChangedType.Added, slot_));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_"></param>
        public void RemoveSlotById(int id_)
        {
            foreach (var slot in m_Slots)
            {
                if (slot.ID == id_)
                {
                    m_Slots.Remove(slot);

                    if (FunctionSlotChanged != null)
                    {
                        FunctionSlotChanged(this, new FunctionSlotChangedEventArg(FunctionSlotChangedType.Removed, slot));
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSlotCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Inputs");
            OnPropertyChanged("Outputs");
        }

        #region Persistence

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public override void Load(XmlNode node_)
        {
            base.Load(node_);

            foreach (XmlNode node in node_.SelectNodes("SlotList/Slot"))
            {
                int id = int.Parse(node.Attributes["id"].Value);
                FunctionSlotType type = (FunctionSlotType) Enum.Parse(typeof(FunctionSlotType), node.Attributes["type"].Value);

                if (m_NextSlotId <= id) m_NextSlotId = id + 1;

                SequenceFunctionSlot slot = new SequenceFunctionSlot(id, type);
                slot.Name = node.Attributes["name"].Value;
                slot.IsArray = bool.Parse(node.Attributes["isArray"].Value);
                slot.VariableType = Type.GetType(node.Attributes["varType"].Value);

                m_Slots.Add(slot);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public override void Save(XmlNode node_)
        {
            base.Save(node_);

            XmlNode graphNode = node_.SelectSingleNode("Graph[@id='" + ID + "']");
            graphNode.AddAttribute("type", XmlAttributeTypeValue);

            XmlNode slotListNode = node_.OwnerDocument.CreateElement("SlotList");
            graphNode.AppendChild(slotListNode);

            //save slots
            foreach (SequenceFunctionSlot s in m_Slots)
            {
                XmlNode slotNode = node_.OwnerDocument.CreateElement("Slot");
                slotListNode.AppendChild(slotNode);

                slotNode.AddAttribute("type", Enum.GetName(typeof(FunctionSlotType), s.SlotType));
                slotNode.AddAttribute("varType", s.VariableType.FullName);
                slotNode.AddAttribute("isArray", s.IsArray.ToString());
                slotNode.AddAttribute("name", s.Name);
                slotNode.AddAttribute("id", s.ID.ToString());
            }
        }

        #endregion // Persistence

        #endregion //Methods
    }
}
