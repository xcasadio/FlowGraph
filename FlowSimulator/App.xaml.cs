using System;
using System.Windows;
using FlowGraphBase.Logger;

namespace FlowSimulator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
#if !DEBUG

        bool m_ShowFatalError = true;

#endif //!DEBUG

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_DispatcherUnhandledException(object sender,
                       System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Exception ex = Helper.GetFirstException(e.Exception);

            LogManager.Instance.WriteException(ex);

#if DEBUG
            MessageBox.Show("Application_DispatcherUnhandledException() : " + ex.Message + Environment.NewLine + ex.StackTrace,
                "Exception Caught", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
#else
            if (m_ShowFatalError == true)
            {
                MessageBox.Show(
                    "Fatal error! The application is going to close! (see log)", 
                    "Uncaught Exception", 
                    MessageBoxButton.Ok, MessageBoxImage.Error);
                e.Handled = false;
                m_ShowFatalError = false;
            }            
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException +=
                  new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception)
            {
                Exception ex = Helper.GetFirstException(e.ExceptionObject as Exception);
#if DEBUG
                MessageBox.Show(ex.Message, "Uncaught Thread Exception." + ex.Message + "\n" + ex.StackTrace,
                                MessageBoxButton.OK, MessageBoxImage.Error);
#endif
                LogManager.Instance.WriteException(ex);
            }
        }
    }
}
