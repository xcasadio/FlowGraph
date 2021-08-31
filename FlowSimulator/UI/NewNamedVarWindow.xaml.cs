using System.Windows;
using System.ComponentModel;
using FlowGraphBase;

namespace FlowSimulator.UI
{
    /// <summary>
    /// Interaction logic for NewNamedVarWindow.xaml
    /// </summary>
    public partial class NewNamedVarWindow : Window
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
        public string InputType
        {
            get => comboBox.SelectedItem.ToString();
            set => comboBox.SelectedItem = value;
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
        public NewNamedVarWindow(NamedVariable var_ = null)
        {
            InitializeComponent();

            if (var_ == null)
            {
                Title = "New named variable";
                comboBox.SelectedIndex = 0;
            }
            else
            {
                InputName = var_.Name;
                comboBox.IsEnabled = false;
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
