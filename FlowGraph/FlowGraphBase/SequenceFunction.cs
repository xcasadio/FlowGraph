using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Xml;
using FlowGraphBase.Node.StandardActionNode;
using FlowGraphBase.Node.StandardEventNode;

namespace FlowGraphBase
{
    /// <summary>
    /// 
    /// </summary>
    public class SequenceFunction : SequenceBase
    {
        public const string XmlAttributeTypeValue = "Function";

        public event EventHandler<FunctionSlotChangedEventArg> FunctionSlotChanged;

        private readonly ObservableCollection<SequenceFunctionSlot> _Slots = new ObservableCollection<SequenceFunctionSlot>();
        private int _NextSlotId;

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<SequenceFunctionSlot> Inputs
        {
            get 
            {
                foreach (SequenceFunctionSlot s in _Slots)
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
                foreach (SequenceFunctionSlot s in _Slots)
                {
                    if (s.SlotType == FunctionSlotType.Output)
                    {
                        yield return s;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public SequenceFunction(string name_)
            : base(name_)
        {
            AddNode(new OnEnterFunctionEvent(this));
            AddNode(new ReturnNode(this));

            _Slots.CollectionChanged += OnSlotCollectionChanged;
        }

        /// <summary>
        /// 
        /// </summary>
        public SequenceFunction(XmlNode node_)
            : base(node_)
        {
            _Slots.CollectionChanged += OnSlotCollectionChanged;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name_"></param>
        public void AddInput(string name_)
        {
            AddSlot(new SequenceFunctionSlot(++_NextSlotId, FunctionSlotType.Input) { Name = name_ });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name_"></param>
        public void AddOutput(string name_)
        {
            AddSlot(new SequenceFunctionSlot(++_NextSlotId, FunctionSlotType.Output) { Name = name_ });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slot_"></param>
        private void AddSlot(SequenceFunctionSlot slot_)
        {
            slot_.IsArray = false;
            slot_.VariableType = typeof(int);

            _Slots.Add(slot_);

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
            foreach (var slot in _Slots)
            {
                if (slot.ID == id_)
                {
                    _Slots.Remove(slot);

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node_"></param>
        public override void Load(XmlNode node_)
        {
            base.Load(node_);

            foreach (XmlNode slotNode in node_.SelectNodes("SlotList/Slot"))
            {
                int id = int.Parse(slotNode.Attributes["id"].Value);
                FunctionSlotType type = (FunctionSlotType) Enum.Parse(typeof(FunctionSlotType), slotNode.Attributes["type"].Value);

                if (_NextSlotId <= id) _NextSlotId = id + 1;

                SequenceFunctionSlot slot = new SequenceFunctionSlot(id, type)
                {
                    Name = slotNode.Attributes["name"].Value,
                    IsArray = bool.Parse(slotNode.Attributes["isArray"].Value),
                    VariableType = Type.GetType(slotNode.Attributes["varType"].Value)
                };

                _Slots.Add(slot);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public override void Save(XmlNode node)
        {
            base.Save(node);

            XmlNode graphNode = node.SelectSingleNode("Graph[@id='" + Id + "']");
            graphNode.AddAttribute("type", XmlAttributeTypeValue);

            XmlNode slotListNode = node.OwnerDocument.CreateElement("SlotList");
            graphNode.AppendChild(slotListNode);

            //save slots
            foreach (SequenceFunctionSlot s in _Slots)
            {
                XmlNode slotNode = node.OwnerDocument.CreateElement("Slot");
                slotListNode.AppendChild(slotNode);

                slotNode.AddAttribute("type", Enum.GetName(typeof(FunctionSlotType), s.SlotType));
                slotNode.AddAttribute("varType", s.VariableType.FullName);
                slotNode.AddAttribute("isArray", s.IsArray.ToString());
                slotNode.AddAttribute("name", s.Name);
                slotNode.AddAttribute("id", s.ID.ToString());
            }
        }
    }
}
