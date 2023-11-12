using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using FlowGraph.Logger;
using FlowSimulator.Logger;

namespace FlowSimulator.UI
{
    public partial class LogViewer : UserControl
    {
        ScrollViewer _scrollViewer;
        bool _isAutoScroll = true;

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

        public LogViewer()
        {
            LogManager.Instance.AddLogger(new LogEditor());
            InitializeComponent();

            LogEditor.LogEntries.CollectionChanged += OnCollectionChanged;
        }

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_scrollViewer == null)
            {
                var o = logContent.Template.FindName("logScrollViewer", logContent);

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

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            LogEditor.LogEntries.Clear();
        }
    }
}
