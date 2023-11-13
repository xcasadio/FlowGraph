using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FlowGraph;

namespace FlowGraphUI;

public partial class FlowGraphDataControl : UserControl
{
    private Point _dragStartPoint;
    private bool _isDragAndDrop;

    public FlowGraphViewModel FlowGraphViewModel => DataContext as FlowGraphViewModel;

    public FlowGraphDataControl()
    {
        InitializeComponent();
    }

    private void ListBoxGraphIte_MouseDoubleClick(object sender, MouseButtonEventArgs arg)
    {
        var cmd = (RoutedUICommand)Application.Current.Resources["Commands.OpenGraph"];
        cmd.Execute(arg, this);
    }

    private void listBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (ItemsControl.ContainerFromElement(sender as ListBox, e.OriginalSource as DependencyObject) is ListBoxItem item)
        {
            Debugger.Break();
            //MainWindow.Instance.DetailsControl.DataContext = item.DataContext;
        }
    }

    private void ListBoxGraphFunctionItem_MouseDoubleClick(object sender, MouseButtonEventArgs arg)
    {
        var cmd = (RoutedUICommand)Application.Current.Resources["Commands.OpenFunction"];
        cmd.Execute(arg, this);
    }

    private void OpenFunction_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (listBoxGraphFunctions.SelectedItem is SequenceFunction item)
        {
            Debugger.Break();
            //MainWindow.Instance.FlowGraphManagerControl.OpenGraphInNewTab(item);
        }
    }

    private void CreateFunction_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        var win = new SequenceParametersWindow
        {
            Title = "New Function parameters",
            InputName = "name of the function",
            InputDescription = "",
            IsValidInputNameCallback = FlowGraphViewModel.IsValidFunctionName,
            Owner = Application.Current.MainWindow
        };

        if (win.ShowDialog() == false)
        {
            return;
        }

        var newSeq = new SequenceFunction(win.InputName)
        {
            Description = win.InputDescription
        };

        var sequenceViewModel = new SequenceViewModel(newSeq);

        FlowGraphViewModel.AddFunction(sequenceViewModel);
        sequenceViewModel.InitialNodeFromNewFunction();

        Debugger.Break();
        //MainWindow.Instance.FlowGraphManagerControl.OpenGraphInNewTab(newSeq);
    }

    private void RenameFunction_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (listBoxGraphFunctions.SelectedItem is SequenceViewModel sequenceViewModel)
        {
            var win = new SequenceParametersWindow
            {
                Title = "Function " + sequenceViewModel.Name + " parameters",
                InputName = sequenceViewModel.Name,
                InputDescription = sequenceViewModel.Description,
                IsValidInputNameCallback = FlowGraphViewModel.IsValidFunctionName,
                Owner = Application.Current.MainWindow
            };

            if (win.ShowDialog() == false)
            {
                return;
            }

            sequenceViewModel.Name = win.InputName;
            sequenceViewModel.Description = win.InputDescription;
        }
    }

    private void DeleteFunction_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (listBoxGraphFunctions.SelectedItem is SequenceViewModel sequenceViewModel)
        {
            if (MessageBox.Show(
                    "Do you really want to delete the function " + sequenceViewModel.Name + " ?",
                    "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Debugger.Break();
                //MainWindow.Instance.FlowGraphManagerControl.CloseTab(sequenceViewModel);
                FlowGraphViewModel.RemoveFunction(sequenceViewModel);
            }
        }
    }

    private void listBoxGraphFunctions_PreviewMouseLeftButton(object sender, MouseButtonEventArgs e)
    {
        listBox_PreviewMouseLeftButtonDown(sender, e);

        if (e.ButtonState == MouseButtonState.Pressed)
        {
            _dragStartPoint = e.GetPosition(null);
            _isDragAndDrop = true;
        }
        else if (e.ButtonState == MouseButtonState.Released)
        {
            _isDragAndDrop = false;
        }
    }

    private void listBoxGraphFunctions_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        if (_isDragAndDrop)
        {
            // Get the current mouse position
            var mousePos = e.GetPosition(null);
            var diff = _dragStartPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                sender is ListBox listBox &&
                e.OriginalSource is DependencyObject source &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                 Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                var listBoxItem =
                    DependencyObjectHelper.FindAnchestor<ListBoxItem>(source);

                if (listBoxItem != null)
                {
                    var func = (SequenceFunction)listBox.ItemContainerGenerator.
                        ItemFromContainer(listBoxItem);

                    if (func != null)
                    {
                        var dragData = new DataObject(DataFormats.Text, FlowGraphDragAndDropManager.DragPrefixFunction + func.Id);
                        DragDrop.DoDragDrop(listBoxItem, dragData, DragDropEffects.Move);
                    }
                }
            }
        }
    }

    private void CreateNamedVar_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        var win = new NewNamedVarWindow
        {
            Title = "New Named Variable",
            InputName = "name of the variable",
            IsValidInputNameCallback = NamedVariableManager.Instance.IsValidName,
            Owner = Application.Current.MainWindow
        };

        if (win.ShowDialog() == false)
        {
            return;
        }

        NamedVariableManager.Instance.Add(
            win.InputName,
            VariableTypeInspector.CreateDefaultValueFromType(Type.GetType(win.InputType)));
    }

    private void RenameNamedVar_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (listBoxGraphNamedVars.SelectedItem is NamedVariable var)
        {
            var win = new NewNamedVarWindow(var)
            {
                Title = "Rename Named Variable",
                IsValidInputNameCallback = NamedVariableManager.Instance.IsValidName,
                Owner = Application.Current.MainWindow
            };

            if (win.ShowDialog() == false)
            {
                return;
            }

            var.Name = win.InputName;
        }
    }

    private void DeleteNamedVar_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        if (listBoxGraphNamedVars.SelectedItem is NamedVariable variable)
        {
            if (MessageBox.Show(
                    "Do you really want to delete the named variable " + variable.Name + " ?",
                    "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                NamedVariableManager.Instance.Remove(variable);
            }
        }
    }

    private void listBoxGraphNamedVars_PreviewMouseLeftButton(object sender, MouseButtonEventArgs e)
    {
        listBox_PreviewMouseLeftButtonDown(sender, e);

        if (e.ButtonState == MouseButtonState.Pressed)
        {
            _dragStartPoint = e.GetPosition(null);
            _isDragAndDrop = true;
        }
        else if (e.ButtonState == MouseButtonState.Released)
        {
            _isDragAndDrop = false;
        }
    }

    private void listBoxGraphNamedVars_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        if (_isDragAndDrop)
        {
            // Get the current mouse position
            var mousePos = e.GetPosition(null);
            var diff = _dragStartPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                sender is ListBox listBox &&
                e.OriginalSource is DependencyObject source &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                 Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                var listBoxItem = DependencyObjectHelper.FindAnchestor<ListBoxItem>(source);

                if (listBoxItem != null)
                {
                    var var = (NamedVariable)listBox.ItemContainerGenerator.
                        ItemFromContainer(listBoxItem);

                    if (var != null)
                    {
                        var dragData = new DataObject(DataFormats.Text, FlowGraphDragAndDropManager.DragPrefixNamedVar + var.Name);
                        DragDrop.DoDragDrop(listBoxItem, dragData, DragDropEffects.Move);
                    }
                }
            }
        }
    }
}