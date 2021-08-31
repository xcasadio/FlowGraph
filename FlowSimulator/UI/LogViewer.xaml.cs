using System.Windows;
using System.Windows.Controls;
using FlowSimulator.Logger;
using FlowGraphBase.Logger;
using System.Collections.Specialized;

namespace FlowSimulator.UI
{
    /// <summary>
    /// Interaction logic for LogViewer.xaml
    /// </summary>
    public partial class LogViewer : UserControl
    {
		#region Fields

        ScrollViewer m_ScrollViewer = null;
        bool m_IsAutoScroll = true;

		#endregion //Fields
	
		#region Properties

        /// <summary>
        /// 
        /// </summary>
        public bool IsAutoScroll
        {
            get => m_IsAutoScroll;
            set 
            {
                if (m_IsAutoScroll != value)
                {
                    m_IsAutoScroll = value;

                    if (m_IsAutoScroll
                        && m_ScrollViewer != null)
                    {
                        m_ScrollViewer.ScrollToBottom();
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

            LogEditor.LogEntries.CollectionChanged += new NotifyCollectionChangedEventHandler(OnCollectionChanged);
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
            if (m_ScrollViewer == null)
            {
                object o = logContent.Template.FindName("logScrollViewer", logContent);

                if (o != null && o is ScrollViewer)
                {
                    m_ScrollViewer = o as ScrollViewer;
                }
            }

            if (m_ScrollViewer != null &&
                m_IsAutoScroll)
            {
                m_ScrollViewer.ScrollToBottom();
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
