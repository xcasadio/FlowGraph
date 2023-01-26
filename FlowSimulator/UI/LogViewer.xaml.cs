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
        ScrollViewer _scrollViewer;
        bool _isAutoScroll = true;

        /// <summary>
        /// 
        /// </summary>
        public bool IsAutoScroll
        {
            get => _isAutoScroll;
            set
            {
                if (_isAutoScroll != value)
                {
                    _isAutoScroll = value;

                    if (_isAutoScroll
                        && _scrollViewer != null)
                    {
                        _scrollViewer.ScrollToBottom();
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public LogViewer()
        {
            LogManager.Instance.AddLogger(new LogEditor());
            InitializeComponent();

            LogEditor.LogEntries.CollectionChanged += OnCollectionChanged;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_scrollViewer == null)
            {
                object o = logContent.Template.FindName("logScrollViewer", logContent);

                if (o != null && o is ScrollViewer viewer)
                {
                    _scrollViewer = viewer;
                }
            }

            if (_scrollViewer != null &&
                _isAutoScroll)
            {
                _scrollViewer.ScrollToBottom();
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
    }
}
