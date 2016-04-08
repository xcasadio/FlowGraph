using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FlowSimulator.FlowGraphs;
using FlowGraphBase;

namespace FlowSimulator.UI
{
    /// <summary>
    /// Interaction logic for FlowGraphManagerControl.xaml
    /// </summary>
    public partial class FlowGraphManagerControl : UserControl
    {
		#region Fields

        public event EventHandler<EventArg1Param<SequenceBase>> SelectedGraphChanged; 

		#endregion //Fields
	
		#region Properties
		
		#endregion //Properties
	
		#region Constructors

        /// <summary>
        /// 
        /// </summary>
        public FlowGraphManagerControl()
        {
            InitializeComponent();
        }

		#endregion //Constructors
	
		#region Methods

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
            FlowGraphControlViewModel fgvm = FlowGraphManager.Instance.GetViewModelByID(seq_.Id);
            tab.DataContext = fgvm;

            FlowGraphControl fgc = new FlowGraphControl();
            fgc.DataContext = fgvm;
            tab.Content = fgc;

            Binding bind = new Binding("Name");
            bind.Source = fgvm;
            tab.SetBinding(TabItem.HeaderProperty, bind);

            tabControl.SelectedIndex = tabControl.Items.Add(tab);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm_"></param>
//         public void OpenGraphInNewTab(FlowGraphControlViewModel vm_)
//         {
//             foreach (TabItem item in tabControl.Items)
//             {
//                 FlowGraphControlViewModel fgvm = item.DataContext as FlowGraphControlViewModel;
//                 if (fgvm.Id == vm_.Id)
//                 {
//                     tabControl.SelectedItem = item;
//                     return;
//                 }
//             }
// 
//             TabItem tab = new TabItem();
//             tab.DataContext = vm_;
//             FlowGraphControl fgc = new FlowGraphControl();
//             fgc.DataContext = vm_;
//             tab.Content = fgc;
// 
//             Binding bind = new Binding("Name");
//             bind.Source = vm_;
//             tab.SetBinding(TabItem.HeaderProperty, bind);
// 
//             tabControl.SelectedIndex = tabControl.Items.Add(tab);
//         }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm_"></param>
        public void CloseTab(FlowGraphControlViewModel vm_)
        {
            foreach (TabItem item in tabControl.Items)
            {
                FlowGraphControlViewModel fgvm = item.DataContext as FlowGraphControlViewModel;
                if (fgvm.ID == vm_.ID)
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
            TabItem tab = tabControl.SelectedItem as TabItem;

            if (tab != null)
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

                if (tabControl.SelectedItem is TabItem)
                {
                    TabItem item = (tabControl.SelectedItem as TabItem);

                    if (item.Content is FlowGraphControl)
                    {
                        seq = (item.Content as FlowGraphControl).ViewModel.Sequence;
                    }
                }

                SelectedGraphChanged(this, new EventArg1Param<SequenceBase>(seq));
            }
        }

		#endregion //Methods
    }
}
