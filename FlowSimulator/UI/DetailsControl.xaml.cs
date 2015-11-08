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
using FlowGraphBase;
using FlowGraphBase.Script;

namespace FlowSimulator.UI
{
    /// <summary>
    /// Interaction logic for DetailsControl.xaml
    /// </summary>
    public partial class DetailsControl : UserControl
    {
		#region Fields

		#endregion //Fields
	
		#region Properties

		#endregion //Properties
	
		#region Constructors

        /// <summary>
        /// 
        /// </summary>
        public DetailsControl()
        {
            InitializeComponent();
        }

		#endregion //Constructors
	
		#region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonAddInput_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is SequenceFunction)
            {
                SequenceFunction func = DataContext as SequenceFunction;
                func.AddInput("input");
            }
            else if (DataContext is ScriptElement)
            {
                ScriptElement el = DataContext as ScriptElement;
                el.AddInput("input");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonAddOutput_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is SequenceFunction)
            {
                SequenceFunction func = DataContext as SequenceFunction;
                func.AddOutput("output");
            }
            else if (DataContext is ScriptElement)
            {
                ScriptElement el = DataContext as ScriptElement;
                el.AddOutput("output");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteSlot_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image)
            {
                Image img = sender as Image;

                if (img.Tag is int)
                {
                    int id = (int)img.Tag;

                    if (DataContext is SequenceFunction)
                    {
                        SequenceFunction func = DataContext as SequenceFunction;
                        func.RemoveSlotById(id);
                    }
                    else if (DataContext is ScriptElement)
                    {
                        ScriptElement el = DataContext as ScriptElement;
                        el.RemoveSlotById(id);
                    }
                }
            }
        }

        #endregion //Methods
    }
}
