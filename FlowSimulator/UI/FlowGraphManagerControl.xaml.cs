using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using FlowGraphBase;
using FlowSimulator.FlowGraphs;

namespace FlowSimulator.UI
{
    /// <summary>
    /// Interaction logic for FlowGraphManagerControl.xaml
    /// </summary>
    public partial class FlowGraphManagerControl : UserControl
    {
        public event EventHandler<EventArg1Param<SequenceBase>> SelectedGraphChanged;

        /// <summary>
        /// 
        /// </summary>
        public FlowGraphManagerControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seq_"></param>
        public void OpenGraphInNewTab(SequenceBase seq_)
        {
            foreach (TabItem item in tabControl.Items)
            {
                if ((item.DataContext as FlowGraphControlViewModel).ID == seq_.Id)
                {
                    tabControl.SelectedItem = item;
                    return;
                }
            }

            TabItem tab = new TabItem();
            FlowGraphControlViewModel fgvm = FlowGraphManager.Instance.GetViewModelById(seq_.Id);
            tab.DataContext = fgvm;

            FlowGraphControl fgc = new FlowGraphControl
            {
                DataContext = fgvm
            };
            tab.Content = fgc;

            Binding bind = new Binding("Name")
            {
                Source = fgvm
            };
            tab.SetBinding(HeaderedContentControl.HeaderProperty, bind);

            tabControl.SelectedIndex = tabControl.Items.Add(tab);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v_"></param>
//         public void OpenGraphInNewTab(FlowGraphControlViewModel v_)
//         {
//             foreach (TabItem item in tabControl.Items)
//             {
//                 FlowGraphControlViewModel fgvm = item.DataContext as FlowGraphControlViewModel;
//                 if (fgvm.Id == v_.Id)
//                 {
//                     tabControl.SelectedItem = item;
//                     return;
//                 }
//             }
// 
//             TabItem tab = new TabItem();
//             tab.DataContext = v_;
//             FlowGraphControl fgc = new FlowGraphControl();
//             fgc.DataContext = v_;
//             tab.Content = fgc;
// 
//             Binding bind = new Binding("Name");
//             bind.Source = v_;
//             tab.SetBinding(TabItem.HeaderProperty, bind);
// 
//             tabControl.SelectedIndex = tabControl.Items.Add(tab);
//         }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v_"></param>
        public void CloseTab(FlowGraphControlViewModel v_)
        {
            foreach (TabItem item in tabControl.Items)
            {
                FlowGraphControlViewModel fgvm = item.DataContext as FlowGraphControlViewModel;
                if (fgvm.ID == v_.ID)
                {
                    tabControl.Items.Remove(item);
                    return;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (tabControl.SelectedItem is TabItem tab)
            {
                int index = tabControl.Items.IndexOf(tab);
                tabControl.Items.Remove(tab);

                if (tabControl.Items.Count > 0)
                {
                    tabControl.SelectedIndex = index > 0 ? index - 1 : 0;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedGraphChanged != null)
            {
                SequenceBase seq = null;

                if (tabControl.SelectedItem is TabItem tabItem)
                {
                    TabItem item = tabItem;

                    if (item.Content is FlowGraphControl control)
                    {
                        seq = control.ViewModel.Sequence;
                    }
                }

                SelectedGraphChanged(this, new EventArg1Param<SequenceBase>(seq));
            }
        }
    }
}
