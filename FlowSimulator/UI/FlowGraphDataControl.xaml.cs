﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FlowGraphBase;
using FlowGraphBase.Script;
using FlowSimulator.FlowGraphs;

namespace FlowSimulator.UI
{
    /// <summary>
    /// Interaction logic for FlowGraphListWindow.xaml
    /// </summary>
    public partial class FlowGraphDataControl : UserControl
    {
        public const string DragPrefixFunction = "SequenceFunction#";
        public const string DragPrefixNamedVar = "NamedVariable#";
        public const string DragPrefixScriptElement = "ScriptElement#";

        private Point _DragStartPoint;
        private bool _IsDragAndDrop;

        /// <summary>
        /// 
        /// </summary>
        public FlowGraphDataControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="?"></param>
        private void ListBoxGraphIte_MouseDoubleClick(object sender, MouseButtonEventArgs arg_)
        {
            RoutedUICommand cmd = (RoutedUICommand)Application.Current.Resources["Commands.OpenGraph"];
            cmd.Execute(arg_, this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ItemsControl.ContainerFromElement(sender as ListBox, e.OriginalSource as DependencyObject) is ListBoxItem item)
            {
                MainWindow.Instance.DetailsControl.DataContext = item.DataContext;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenGraph_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (listBoxGraphs.SelectedItem != null
                && listBoxGraphs.SelectedItem is Sequence item)
            {
                MainWindow.Instance.FlowGraphManagerControl.OpenGraphInNewTab(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateGraph_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SequenceParametersWindow win = new SequenceParametersWindow
            {
                Title = "New Graph parameters",
                InputName = "name of the graph",
                InputDescription = "",
                IsValidInputNameCallback = GraphDataManager.Instance.IsValidSequenceName,
                Owner = MainWindow.Instance
            };

            if (win.ShowDialog() == false)
            {
                return;
            }

            Sequence newSeq = new Sequence(win.InputName)
            {
                Description = win.InputDescription
            };
            GraphDataManager.Instance.AddSequence(newSeq);

            FlowGraphControlViewModel wm = new FlowGraphControlViewModel(newSeq);
            FlowGraphManager.Instance.Add(wm);

            MainWindow.Instance.FlowGraphManagerControl.OpenGraphInNewTab(newSeq);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RenameGraph_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (listBoxGraphs.SelectedItem != null
                && listBoxGraphs.SelectedItem is Sequence sequence)
            {
                FlowGraphControlViewModel flowGraphVM = 
                    FlowGraphManager.Instance.GetViewModelById(sequence.Id);

                SequenceParametersWindow win = new SequenceParametersWindow
                {
                    Title = "Graph " + flowGraphVM.Name + " parameters",
                    InputName = flowGraphVM.Name,
                    InputDescription = flowGraphVM.Description,
                    IsValidInputNameCallback = GraphDataManager.Instance.IsValidSequenceName,
                    Owner = MainWindow.Instance
                };

                if (win.ShowDialog() == false)
                {
                    return;
                }

                flowGraphVM.Sequence.Name = win.InputName;
                flowGraphVM.Sequence.Description = win.InputDescription;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteGraph_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (listBoxGraphs.SelectedItem != null
                && listBoxGraphs.SelectedItem is Sequence sequence)
            {
                FlowGraphControlViewModel flowGraphVM =
                    FlowGraphManager.Instance.GetViewModelById(sequence.Id);

                if (MessageBox.Show(
                        "Do you really want to delete the graph " + flowGraphVM.Name + " ?",
                        "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    MainWindow.Instance.FlowGraphManagerControl.CloseTab(flowGraphVM);
                    FlowGraphManager.Instance.Remove(flowGraphVM);
                    GraphDataManager.Instance.RemoveSequence(flowGraphVM.Sequence as Sequence);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="?"></param>
        private void ListBoxGraphFunctionIte_MouseDoubleClick(object sender, MouseButtonEventArgs arg_)
        {
            RoutedUICommand cmd = (RoutedUICommand)Application.Current.Resources["Commands.OpenFunction"];
            cmd.Execute(arg_, this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFunction_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (listBoxGraphFunctions.SelectedItem != null
                && listBoxGraphFunctions.SelectedItem is SequenceFunction item)
            {
                MainWindow.Instance.FlowGraphManagerControl.OpenGraphInNewTab(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateFunction_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SequenceParametersWindow win = new SequenceParametersWindow
            {
                Title = "New Function parameters",
                InputName = "name of the function",
                InputDescription = "",
                IsValidInputNameCallback = GraphDataManager.Instance.IsValidFunctionName,
                Owner = MainWindow.Instance
            };

            if (win.ShowDialog() == false)
            {
                return;
            }

            SequenceFunction newSeq = new SequenceFunction(win.InputName)
            {
                Description = win.InputDescription
            };
            GraphDataManager.Instance.AddFunction(newSeq);

            FlowGraphControlViewModel wm = new FlowGraphControlViewModel(newSeq);
            wm.InitialNodeFromNewFunction();
            FlowGraphManager.Instance.Add(wm);

            MainWindow.Instance.FlowGraphManagerControl.OpenGraphInNewTab(newSeq);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RenameFunction_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (listBoxGraphFunctions.SelectedItem != null
                && listBoxGraphFunctions.SelectedItem is SequenceFunction function)
            {
                FlowGraphControlViewModel flowGraphVM =
                    FlowGraphManager.Instance.GetViewModelById(function.Id);

                SequenceParametersWindow win = new SequenceParametersWindow
                {
                    Title = "Function " + flowGraphVM.Name + " parameters",
                    InputName = flowGraphVM.Name,
                    InputDescription = flowGraphVM.Description,
                    IsValidInputNameCallback = GraphDataManager.Instance.IsValidFunctionName,
                    Owner = MainWindow.Instance
                };

                if (win.ShowDialog() == false)
                {
                    return;
                }

                flowGraphVM.Sequence.Name = win.InputName;
                flowGraphVM.Sequence.Description = win.InputDescription;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteFunction_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (listBoxGraphFunctions.SelectedItem != null
                && listBoxGraphFunctions.SelectedItem is SequenceFunction function)
            {
                FlowGraphControlViewModel flowGraphVM =
                    FlowGraphManager.Instance.GetViewModelById(function.Id);

                if (MessageBox.Show(
                        "Do you really want to delete the function " + flowGraphVM.Name + " ?",
                        "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    MainWindow.Instance.FlowGraphManagerControl.CloseTab(flowGraphVM);
                    FlowGraphManager.Instance.Remove(flowGraphVM);
                    GraphDataManager.Instance.RemoveFunction(flowGraphVM.Sequence as SequenceFunction);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxGraphFunctions_PreviewMouseLeftButton(object sender, MouseButtonEventArgs e)
        {
            listBox_PreviewMouseLeftButtonDown(sender, e);

            if (e.ButtonState == MouseButtonState.Pressed)
            {
                _DragStartPoint = e.GetPosition(null);
                _IsDragAndDrop = true;
            }
            else if (e.ButtonState == MouseButtonState.Released)
            {
                _IsDragAndDrop = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxGraphFunctions_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_IsDragAndDrop)
            {
                // Get the current mouse position
                Point mousePos = e.GetPosition(null);
                Vector diff = _DragStartPoint - mousePos;

                if (e.LeftButton == MouseButtonState.Pressed &&
                    sender is ListBox listBox &&
                    e.OriginalSource is DependencyObject source &&
                    (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
                {
                    ListBoxItem listBoxItem =
                        DependencyObjectHelper.FindAnchestor<ListBoxItem>(source);

                    if (listBoxItem != null)
                    {
                        SequenceFunction func = (SequenceFunction)listBox.ItemContainerGenerator.
                            ItemFromContainer(listBoxItem);

                        if (func != null)
                        {
                            DataObject dragData = new DataObject(DataFormats.Text, DragPrefixFunction + func.Id);
                            DragDrop.DoDragDrop(listBoxItem, dragData, DragDropEffects.Move);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNamedVar_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NewNamedVarWindow win = new NewNamedVarWindow
            {
                Title = "New Named Variable",
                InputName = "name of the variable",
                IsValidInputNameCallback = NamedVariableManager.Instance.IsValidName,
                Owner = MainWindow.Instance
            };

            if (win.ShowDialog() == false)
            {
                return;
            }

            NamedVariableManager.Instance.Add(
                win.InputName, 
                VariableTypeInspector.CreateDefaultValueFromType(Type.GetType(win.InputType)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RenameNamedVar_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (listBoxGraphNamedVars.SelectedItem != null
                && listBoxGraphNamedVars.SelectedItem is NamedVariable @var)
            {
                NewNamedVarWindow win = new NewNamedVarWindow(var)
                {
                    Title = "Rename Named Variable",
                    IsValidInputNameCallback = NamedVariableManager.Instance.IsValidName,
                    Owner = MainWindow.Instance
                };

                if (win.ShowDialog() == false)
                {
                    return;
                }

                var.Name = win.InputName;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteNamedVar_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (listBoxGraphNamedVars.SelectedItem != null
                && listBoxGraphNamedVars.SelectedItem is NamedVariable variable)
            {
                if (MessageBox.Show(
                        "Do you really want to delete the named variable " + variable.Name + " ?",
                        "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    NamedVariableManager.Instance.Remove(variable);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxGraphNamedVars_PreviewMouseLeftButton(object sender, MouseButtonEventArgs e)
        {
            listBox_PreviewMouseLeftButtonDown(sender, e);

            if (e.ButtonState == MouseButtonState.Pressed)
            {
                _DragStartPoint = e.GetPosition(null);
                _IsDragAndDrop = true;
            }
            else if (e.ButtonState == MouseButtonState.Released)
            {
                _IsDragAndDrop = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxGraphNamedVars_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_IsDragAndDrop)
            {
                // Get the current mouse position
                Point mousePos = e.GetPosition(null);
                Vector diff = _DragStartPoint - mousePos;

                if (e.LeftButton == MouseButtonState.Pressed &&
                    sender is ListBox listBox &&
                    e.OriginalSource is DependencyObject source &&
                    (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
                {
                    ListBoxItem listBoxItem =
                        DependencyObjectHelper.FindAnchestor<ListBoxItem>(source);

                    if (listBoxItem != null)
                    {
                        NamedVariable var = (NamedVariable)listBox.ItemContainerGenerator.
                            ItemFromContainer(listBoxItem);

                        if (var != null)
                        {
                            DataObject dragData = new DataObject(DataFormats.Text, DragPrefixNamedVar + var.Name);
                            DragDrop.DoDragDrop(listBoxItem, dragData, DragDropEffects.Move);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="?"></param>
        private void ListBoxGraphScriptIte_MouseDoubleClick(object sender, MouseButtonEventArgs arg_)
        {
            RoutedUICommand cmd = (RoutedUICommand)Application.Current.Resources["Commands.OpenScript"];
            cmd.Execute(arg_, this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenScript_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (listBoxGraphScripts.SelectedItem != null
                && listBoxGraphScripts.SelectedItem is ScriptElement item)
            {
                MainWindow.Instance.OpenScriptElement(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateScript_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NewScriptWindow win = new NewScriptWindow
            {
                Title = "New script",
                InputName = "name of the script",
                //win.IsValidInputNameCallback = GraphDataManager.Instance.IsValidName;
                Owner = MainWindow.Instance
            };

            if (win.ShowDialog() == false)
            {
                return;
            }

            GraphDataManager.Instance.AddScript(new ScriptElement { Name = win.InputName });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RenameScript_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (listBoxGraphScripts.SelectedItem != null
                && listBoxGraphScripts.SelectedItem is ScriptElement el)
            {
                NewScriptWindow win = new NewScriptWindow(el)
                {
                    //win.IsValidInputNameCallback = GraphDataManager.Instance.IsValidName;
                    Owner = MainWindow.Instance
                };

                if (win.ShowDialog() == false)
                {
                    return;
                }

                el.Name = win.InputName;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteScript_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (listBoxGraphScripts.SelectedItem != null
                && listBoxGraphScripts.SelectedItem is ScriptElement el)
            {
                if (MessageBox.Show(
                        "Do you really want to delete the named variable " + el.Name + " ?",
                        "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    GraphDataManager.Instance.RemoveScript(el);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxGraphScripts_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_IsDragAndDrop)
            {
                // Get the current mouse position
                Point mousePos = e.GetPosition(null);
                Vector diff = _DragStartPoint - mousePos;

                if (e.LeftButton == MouseButtonState.Pressed &&
                    sender is ListBox listBox &&
                    e.OriginalSource is DependencyObject source &&
                    (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
                {
                    ListBoxItem listBoxItem =
                        DependencyObjectHelper.FindAnchestor<ListBoxItem>(source);

                    if (listBoxItem != null)
                    {
                        ScriptElement var = (ScriptElement)listBox.ItemContainerGenerator.
                            ItemFromContainer(listBoxItem);

                        if (var != null)
                        {
                            DataObject dragData = new DataObject(DataFormats.Text, DragPrefixScriptElement + var.Id);
                            DragDrop.DoDragDrop(listBoxItem, dragData, DragDropEffects.Move);
                        }
                    }
                }
            }
        }
    }
}
