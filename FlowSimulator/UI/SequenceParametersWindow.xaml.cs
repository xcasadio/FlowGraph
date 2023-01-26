using System.ComponentModel;
using System.Windows;

namespace FlowSimulator.UI
{
    /// <summary>
    /// Interaction logic for SequenceParametersWindow.xaml
    /// </summary>
    public partial class SequenceParametersWindow
    {
        public delegate bool IsValidInputNameDelegate(string name);

        private bool _dialogResult;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        public SequenceParametersWindow(FlowGraphControlViewModel vm = null, IsValidInputNameDelegate callback = null)
        {
            InitializeComponent();

            IsValidInputNameCallback = callback;

            if (vm == null)
            {
                Title = "New Graph parameters";
            }
            else
            {
                Title = "Graph " + vm.Name + " parameters";
                textBoxName.Text = vm.Name;
                textBoxDescription.Text = vm.Description;
            }

            Closing += OnClosing;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnClosing(object sender, CancelEventArgs e)
        {
            DialogResult = _dialogResult;
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
                _dialogResult = true;
                Close();
            }
            else
            {
                _dialogResult = false;
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
            _dialogResult = false;
            Close();
        }
    }
}
