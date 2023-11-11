using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FlowGraph;
using FlowGraph.Script;

namespace FlowSimulator.UI
{
    public partial class DetailsControl : UserControl
    {
        public DetailsControl()
        {
            InitializeComponent();
        }

        private void ButtonAddInput_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is SequenceFunction func)
            {
                func.AddInput("input");
            }
            else if (DataContext is ScriptElement el)
            {
                el.AddInput("input");
            }
        }

        private void ButtonAddOutput_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is SequenceFunction func)
            {
                func.AddOutput("output");
            }
            else if (DataContext is ScriptElement el)
            {
                el.AddOutput("output");
            }
        }

        private void btnDeleteSlot_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image { Tag: int id })
            {
                if (DataContext is SequenceFunction func)
                {
                    func.RemoveSlotById(id);
                }
                else if (DataContext is ScriptElement el)
                {
                    el.RemoveSlotById(id);
                }
            }
        }
    }
}
