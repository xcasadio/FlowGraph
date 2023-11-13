using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FlowGraph;

namespace FlowGraphUI;

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
    }

    private void ButtonAddOutput_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is SequenceFunction func)
        {
            func.AddOutput("output");
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
        }
    }
}