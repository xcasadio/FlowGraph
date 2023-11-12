using System.Windows;
using System.Windows.Controls;
using FlowGraph;
using FlowGraphUI;

namespace FlowSimulator.UI
{
    public partial class FlowGraphManagerControl : UserControl
    {
        public event EventHandler<EventArg1Param<SequenceBase?>>? SelectedGraphChanged;

        public FlowGraphManagerControl()
        {
            InitializeComponent();

            var flowGraphViewerControlViewModel = new SequenceViewModel(new Sequence("test"));
            var fgc = new FlowGraphControl { DataContext = flowGraphViewerControlViewModel };
            containerFlowGraph.Children.Add(fgc);
        }

        public void OpenGraphInNewTab(SequenceBase seq)
        {


            /*
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

            tabControl.SelectedIndex = tabControl.Items.Add(tab);*/
        }

        public void CloseTab(SequenceViewModel v)
        {
            /*foreach (TabItem item in tabControl.Items)
            {
                if (item.DataContext is FlowGraphViewerControlViewModel fgvm && fgvm.Id == v.Id)
                {
                    tabControl.Items.Remove(item);
                    return;
                }
            }*/
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            /*if (tabControl.SelectedItem is TabItem tab)
            {
                int index = tabControl.Items.IndexOf(tab);
                tabControl.Items.Remove(tab);

                if (tabControl.Items.Count > 0)
                {
                    tabControl.SelectedIndex = index > 0 ? index - 1 : 0;
                }
            }*/
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*if (SelectedGraphChanged == null)
            {
                return;
            }

            SequenceBase? seq = null;

            if (tabControl.SelectedItem is TabItem { Content: FlowGraphControl control } tabItem)
            {
                seq = control.ViewModel.Sequence;
            }

            SelectedGraphChanged(this, new EventArg1Param<SequenceBase?>(seq));*/
        }
    }
}
