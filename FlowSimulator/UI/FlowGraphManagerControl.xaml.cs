using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using FlowGraph;
using FlowGraphUI;
using FlowSimulator.FlowGraphs;

namespace FlowSimulator.UI
{
    public partial class FlowGraphManagerControl : UserControl
    {
        public event EventHandler<EventArg1Param<SequenceBase?>>? SelectedGraphChanged;

        public FlowGraphManagerControl()
        {
            InitializeComponent();
        }

        public void OpenGraphInNewTab(SequenceBase seq)
        {
            foreach (TabItem item in tabControl.Items)
            {
                if (item.DataContext is FlowGraphViewerControlViewModel fgcvm && fgcvm.Id == seq.Id)
                {
                    tabControl.SelectedItem = item;
                    return;
                }
            }

            TabItem tab = new TabItem();
            FlowGraphViewerControlViewModel fgvm = FlowGraphManager.Instance.GetViewModelById(seq.Id);
            tab.DataContext = fgvm;

            FlowGraphControl fgc = new FlowGraphControl { DataContext = fgvm };
            tab.Content = fgc;

            Binding bind = new Binding("Name") { Source = fgvm };
            tab.SetBinding(HeaderedContentControl.HeaderProperty, bind);

            tabControl.SelectedIndex = tabControl.Items.Add(tab);
        }

        public void CloseTab(FlowGraphViewerControlViewModel v)
        {
            foreach (TabItem item in tabControl.Items)
            {
                if (item.DataContext is FlowGraphViewerControlViewModel fgvm && fgvm.Id == v.Id)
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
            if (SelectedGraphChanged == null)
            {
                return;
            }

            SequenceBase? seq = null;

            if (tabControl.SelectedItem is TabItem { Content: FlowGraphControl control } tabItem)
            {
                seq = control.ViewModel.Sequence;
            }

            SelectedGraphChanged(this, new EventArg1Param<SequenceBase?>(seq));
        }
    }
}
