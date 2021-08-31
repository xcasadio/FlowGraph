using System;
using System.Windows;
using System.Windows.Controls;
using FlowGraphBase;
using FlowGraphBase.Node;
using FlowGraphBase.Script;
using NetworkModel;

namespace FlowSimulator.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class VariableNodeTemplateSelector
        : DataTemplateSelector
    {
        public DataTemplate NumericTemplate { get; set; }
        public DataTemplate SelectableTemplate { get; set; }
        public DataTemplate CheckableTemplate { get; set; }
        public DataTemplate TextTemplate { get; set; }
        public DataTemplate ReadOnlyTemplate { get; set; }
        public DataTemplate CustomWindowTemplate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DependencyObject parentObject = container;

            while ((parentObject = parentObject.GetParentObject()) != null)
            {
                if ((parentObject as FrameworkElement).DataContext is NodeViewModel)
                {
                    NodeViewModel node = (parentObject as FrameworkElement).DataContext as NodeViewModel;

                    if (node.SeqNode.NodeType == NodeType.Variable)
                    {
                        VariableNode varNode = node.SeqNode as VariableNode;

                        switch (varNode.Slots[0].ControlType)
                        {
                            case VariableControlType.Numeric:
                                return NumericTemplate;

                            case VariableControlType.Selectable:
                                return SelectableTemplate;

                            case VariableControlType.Checkable:
                                return CheckableTemplate;

                            case VariableControlType.Text:
                                return TextTemplate;

                            case VariableControlType.ReadOnly:
                                return ReadOnlyTemplate;

                            case VariableControlType.Custom:
                                return CustomWindowTemplate;
                        }
                    }
                }
                else if ((parentObject as FrameworkElement).DataContext is NodeSlotVar
                    || (parentObject as FrameworkElement).DataContext is NamedVariable)
                {
                    Type varType = null;
                    
                    if ((parentObject as FrameworkElement).DataContext is NodeSlotVar)
                    {
                        varType = ((parentObject as FrameworkElement).DataContext as NodeSlotVar).VariableType;
                    }
                    else if ((parentObject as FrameworkElement).DataContext is NamedVariable)
                    {
                        varType = ((parentObject as FrameworkElement).DataContext as NamedVariable).VariableType;
                    }

                    if (NamedVarEditTemplateManager.ContainsType(varType))
                    {
                        return NamedVarEditTemplateManager.GetTemplateByType(varType);
                    }
                }
            }

            return base.SelectTemplate(item, container);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DetailTemplateSelector
        : DataTemplateSelector
    {
        public DataTemplate SequenceTemplate { get; set; }
        public DataTemplate SequenceFunctionTemplate { get; set; }
        public DataTemplate VariableTemplate { get; set; }
        public DataTemplate ScriptTemplate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DependencyObject parentObject = container;

            while ((parentObject = parentObject.GetParentObject()) != null)
            {
                if ((parentObject as FrameworkElement).DataContext is Sequence)
                {
                    return SequenceTemplate;
                }

                if ((parentObject as FrameworkElement).DataContext is SequenceFunction)
                {
                    return SequenceFunctionTemplate;
                }
                if ((parentObject as FrameworkElement).DataContext is NamedVariable)
                {
                    return VariableTemplate;
                }
                if ((parentObject as FrameworkElement).DataContext is ScriptElement)
                {
                    return ScriptTemplate;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ActionNodeConnectorTemplateSelector
        : DataTemplateSelector
    {
        public DataTemplate NodeInTemplate { get; set; }
        public DataTemplate NodeOutTemplate { get; set; }
        public DataTemplate VarInTemplate { get; set; }
        public DataTemplate VarOutTemplate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ConnectorViewModel model)
            {
                switch (model.Type)
                {
                    case ConnectorType.Input:
                        return NodeInTemplate;

                    case ConnectorType.VariableInput:
                        return VarInTemplate;

                    case ConnectorType.Output:
                        return NodeOutTemplate;

                    case ConnectorType.VariableOutput:
                        return VarOutTemplate;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}
