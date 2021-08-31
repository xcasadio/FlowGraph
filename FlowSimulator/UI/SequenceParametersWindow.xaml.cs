using System.Windows;
using System.ComponentModel;

namespace FlowSimulator.UI
{
    /// <summary>
    /// Interaction logic for SequenceParametersWindow.xaml
    /// </summary>
    public partial class SequenceParametersWindow : Window
    {
		#region Fields

        public delegate bool IsValidInputNameDelegate(string name_);

        private bool m_DialogResult = false;

		#endregion //Fields
	
		#region Properties

        /// <summary>
        /// 
        /// </summary>
        public string InputName
        {
            get => textBoxName.Text;
            set => textBoxName.Text = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public string InputDescription
        {
            get => textBoxDescription.Text;
            set => textBoxDescription.Text = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public IsValidInputNameDelegate IsValidInputNameCallback
        {
            get;
            set;
        }

		#endregion //Properties
	
		#region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm_"></param>
        public SequenceParametersWindow(FlowGraphControlViewModel vm_ = null, IsValidInputNameDelegate callback_ = null)
        {
            InitializeComponent();

            IsValidInputNameCallback = callback_;

            if (vm_ == null)
            {
                Title = "New Graph parameters";
            }
            else
            {
                Title = "Graph " + vm_.Name + " parameters";
                textBoxName.Text = vm_.Name;
                textBoxDescription.Text = vm_.Description;
            }

            Closing += new CancelEventHandler(OnClosing);
        }

		#endregion //Constructors
	
		#region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnClosing(object sender, CancelEventArgs e)
        {
            DialogResult = m_DialogResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidInputNameCallback == null
                || (IsValidInputNameCallback != null
                    && IsValidInputNameCallback.Invoke(InputName)))
            {
                m_DialogResult = true;
                Close();
            }
            else
            {
                m_DialogResult = false;
                labelError.Content = "'" + InputName + "' is not a valid name. Please enter a valid name.";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            m_DialogResult = false;
            Close();
        }

		#endregion //Methods
    }
}
