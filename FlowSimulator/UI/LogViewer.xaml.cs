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
        ScrollViewer _ScrollViewer;
        bool _IsAutoScroll = true;

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
            if (_ScrollViewer == null)
            {
                object o = logContent.Template.FindName("logScrollViewer", logContent);

                if (o != null && o is ScrollViewer viewer)
                {
                    _ScrollViewer = viewer;
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
    }
}
