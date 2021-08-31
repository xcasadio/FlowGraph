using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using FlowGraphBase.Logger;
using FlowSimulator.Logger;

namespace FlowSimulator.UI
{
    /// <summary>
    /// Interaction logic for LogViewer.xaml
    /// </summary>
    public partial class LogViewer : UserControl
    {
		#region Fields

        ScrollViewer _ScrollViewer;
        bool _IsAutoScroll = true;

		#endregion //Fields
	
		#region Properties

        /// <summary>
        /// 
        /// </summary>
        public bool IsAutoScroll
        {
            get => _IsAutoScroll;
            set 
            {
                if (_IsAutoScroll != value)
                {
                    _IsAutoScroll = value;

                    if (_IsAutoScroll
                        && _ScrollViewer != null)
                    {
                        _ScrollViewer.ScrollToBottom();
                    }
                }
            }
        }

		#endregion //Properties
	
		#region Constructors

        /// <summary>
        /// 
        /// </summary>
        public LogViewer()
        {
            LogManager.Instance.AddLogger(new LogEditor());
            InitializeComponent();

            LogEditor.LogEntries.CollectionChanged += OnCollectionChanged;
        }

		#endregion //Constructors
	
		#region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_ScrollViewer == null)
            {
                object o = logContent.Template.FindName("logScrollViewer", logContent);

                if (o != null && o is ScrollViewer)
                {
                    _ScrollViewer = o as ScrollViewer;
                }
            }

            if (_ScrollViewer != null &&
                _IsAutoScroll)
            {
                _ScrollViewer.ScrollToBottom();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            LogEditor.LogEntries.Clear();
        }

		#endregion //Methods
    }
}
