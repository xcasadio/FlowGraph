using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml;
using FlowGraphBase;
using FlowGraphBase.Logger;
using FlowGraphBase.Process;
using FlowGraphBase.Script;
using FlowSimulator.FlowGraphs;
using FlowSimulator.UI;
using Xceed.Wpf.AvalonDock.Layout;
using Xceed.Wpf.AvalonDock.Layout.Serialization;
using MenuItem = System.Windows.Controls.MenuItem;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace FlowSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string _UserSettingsFile = "userSettings.xml";
        private readonly string _DockSettingsFile = "dockSettings.xml";

        private MruManager _MruManager;
        private const string _RegistryPath = "Software\\Natixis\\FlowSimulator";

        private string _FileOpened = "";

        private double _LastLeft, _LastTop, _LastWidth, _LastHeight;

        /// <summary>
        /// 
        /// </summary>
        internal static MainWindow Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        internal FlowGraphManagerControl FlowGraphManagerControl => flowGraphManagerControl;

        /// <summary>
        /// 
        /// </summary>
        internal DetailsControl DetailsControl => detailsControl;

        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            Instance = this;

            //LogManager.Instance.NbErrorChanged += new EventHandler(OnNbErrorChanged);
            Version ver = Assembly.GetExecutingAssembly().GetName().Version;
            statusLabelVersion.Content = "v" + ver;
            SetTitle();

            LogManager.Instance.WriteLine(LogVerbosity.Info, "FlowSimulator - v{0} started", ver);
            VariableTypeInspector.SetDefaultValues();
            NamedVarEditTemplateManager.Initialize();

            Loaded += OnLoaded;
            Closed += OnClosed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _MruManager = new MruManager();
                _MruManager.Initialize(
                    this,						// owner form
                    menuItemRecentFiles,        // Recent Files menu item
                    menuItemFile,		        // parent
                    _RegistryPath);			// Registry path to keep MRU list

                _MruManager.MruOpenEvent += delegate(object sender_, MruFileOpenEventArgs e_)
                {
                    SaveChangesOnDemand();
                    LoadFile(e_.FileName);
                };

                LoadSettings();

//                 TaskLauncher.TaskLaunch += new EventHandler<EventArg1Param<Task>>(OnTaskLaunch);
//                 TaskLauncher.TaskFinish += new EventHandler<EventArg1Param<Task>>(OnTaskFinish);
//                 TaskLauncher.TaskMessageSent += new EventHandler<EventArg2Params<int, bool>>(OnTaskMessageSent);
//                 TaskLauncher.TaskCountChanged += new EventHandler<EventArg1Param<int>>(OnTaskCountChanged);
//                 _ReportControl.StartReporting += new EventHandler(OnStartReporting);
//                 _ReportControl.StopReporting += new EventHandler(OnStopReporting);
// 
                if (string.IsNullOrWhiteSpace(_MruManager.GetFirstFileName) == false)
                {
                    LoadFile(_MruManager.GetFirstFileName);
                }

                _LastLeft = Left;
                _LastTop = Top;
                _LastWidth = Width;
                _LastHeight = Height;


                ProcessLauncher.Instance.StartLoop();
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteException(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClosed(object sender, EventArgs e)
        {
            ProcessLauncher.Instance.StopLoop();
            LogManager.Instance.WriteLine(LogVerbosity.Info, "Closed by user");

            try
            {
                SaveSettings();
                //SaveChangesOnDemand();
            }
            catch (Exception ex)
            {
                LogManager.Instance.WriteException(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Resume_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ProcessLauncher.Instance.Resume();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextStep_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ProcessLauncher.Instance.NextStep();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pause_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ProcessLauncher.Instance.Pause();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Stop_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ProcessLauncher.Instance.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NewProject();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenProject();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveProject();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAsCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveAsProject();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemWindows_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            menuItemWindows.Items.Clear();
            SortedList<string, MenuItem> list = new SortedList<string, MenuItem>();

            foreach (var content in dockingManager1.Layout.Descendents().OfType<LayoutContent>())
            {
                MenuItem item = new MenuItem
                {
                    Header = content.Title,
                    IsChecked = content is LayoutDocument ? 
                        (content as LayoutDocument).IsVisible : content is LayoutAnchorable ? 
                            (content as LayoutAnchorable).IsVisible : false
                };
                item.Click += MenuItemLayout_Click;
                item.Tag = content;
                list.Add(content.Title, item);
            }

            foreach (var menu in list)
            {
                menuItemWindows.Items.Add(menu.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemLayout_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            LayoutAnchorable l = item.Tag as LayoutAnchorable;
            l.IsVisible = !l.IsVisible;
            item.IsChecked = l.IsVisible;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemWorkspaceSave_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuIte_OpenDocumentationClick(object sender, EventArgs e)
        {
//             const string fileName = @"FlowSimulator - Manuel utilisateur.pdf";
// 
//             if (File.Exists(fileName) == true)
//             {
//                 System.Diagnostics.Process.Start(fileName);
//             }
//             else
//             {
//                 System.Windows.MessageBox.Show(this, 
//                     "\"" + fileName + "\" not found.", "Error",
//                     MessageBoxButton.Ok, 
//                     MessageBoxImage.Error);
//             }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuIte_HelpClick(object sender, RoutedEventArgs e)
        {
            MessageBox msgBox = new MessageBox();
            windowContainer.Children.Add(msgBox);

            var img = new Image { Source = new BitmapImage( new Uri(
                        "pack://application:,,,/FlowSimulator;component/Resources/Mattahan-Ultrabuuf-Comics-War-Machine.ico"))
            };
            msgBox.ImageSource = img.Source;

            msgBox.ShowMessageBox(
                "Flow Simulator version " + Assembly.GetExecutingAssembly().GetName().Version + Environment.NewLine + 
                "Developed by Xavier Casadio", 
                "Information", 
                MessageBoxButton.OK);
        }

        /// <summary>
        /// 
        /// </summary>
        private void Clear()
        {
            ProjectManager.Clear();
            
            var list = dockingManager1.Layout.Descendents().OfType<LayoutDocument>().ToList();

            foreach (LayoutDocument ld in list)
            {
                ld.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void NewProject()
        {
            SaveChangesOnDemand();

            LogManager.Instance.WriteLine(LogVerbosity.Info, "New project");
            LogManager.Instance.NbErrors = 0;

            _FileOpened = "";

            Clear();
            SetTitle();
        }

        /// <summary>
        /// 
        /// </summary>
        private void OpenProject()
        {
            SaveChangesOnDemand();

            OpenFileDialog form = new OpenFileDialog();
            form.Filter = "Xml files (*.xml)|*.xml";
            form.Multiselect = false;

            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LoadFile(form.FileName);
            }

            form.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        private void SaveProject()
        {
            if (string.IsNullOrWhiteSpace(_FileOpened))
            {
                SaveAsProject();
            }
            else
            {
                SaveFile(_FileOpened);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SaveAsProject()
        {
            SaveFileDialog form = new SaveFileDialog();
            form.Filter = "Xml files (*.xml)|*.xml";

            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SaveFile(form.FileName);
            }

            form.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Exit()
        {
            Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void workSpaceSaveToolStripMenuIte_Click(object sender, EventArgs e)
        {
            SaveSettings();
            LogManager.Instance.WriteLine(LogVerbosity.Debug, "Workspace saved");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fielName_"></param>
        private void LoadFile(string fileName_, bool addToMRU = true)
        {
            if (File.Exists(fileName_))
            {
                Clear();

                if (ProjectManager.OpenFile(fileName_))
                {
                    _FileOpened = fileName_;
                    SetTitle();

                    if (addToMRU)
                    {
                        _MruManager.Add(fileName_);
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show(this, 
                        "Can't load the file '" + fileName_ + "'. Please check the log.",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    _MruManager.Remove(fileName_);
                }
            }
            else
            {
                System.Windows.MessageBox.Show(this, 
                    "The file '" + fileName_ + "' doesn't exist.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _MruManager.Remove(fileName_);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName_"></param>
        private void SaveFile(string fileName_)
        {
            if (ProjectManager.SaveFile(fileName_))
            {
                _MruManager.Add(fileName_);
                _FileOpened = fileName_;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SaveChangesOnDemand()
        {
            if (GraphDataManager.Instance.IsChanges()
                || FlowGraphManager.Instance.IsChanges())
            {
                if (System.Windows.MessageBox.Show(this, "Save changes ?", "Save ?",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    SaveProject();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadSettings()
        {
            double l = Left, t = Top, w = Width, h = Height;
            WindowState winState = WindowState;

            if (File.Exists(_UserSettingsFile))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(_UserSettingsFile);

                XmlNode winNode = xmlDoc.SelectSingleNode("FlowSimulator/Window");

                int version = int.Parse(winNode.Attributes["version"].Value);

                l = int.Parse(winNode.Attributes["left"].Value);
                t = int.Parse(winNode.Attributes["top"].Value);
                w = int.Parse(winNode.Attributes["width"].Value);
                h = int.Parse(winNode.Attributes["height"].Value);
                winState = (WindowState)Enum.Parse(typeof(WindowState), winNode.Attributes["windowState"].Value);

                XmlNode rootNode = xmlDoc.SelectSingleNode("FlowSimulator");

                try
                {
                    //                         _SessionControl.LoadSettings(rootNode);
                    //                         _MessageControl.LoadSettings(rootNode);
                    //                         _TaskControl.LoadSettings(rootNode);
                    //                         _ReportControl.LoadSettings(rootNode);
                    //                         //_ScriptControl.LoadSettings(rootNode);
                    //                         _LogControl.LoadSettings(rootNode);
                }
                catch (Exception ex2)
                {
                    LogManager.Instance.WriteException(ex2);
                }
            }

            if (File.Exists(_DockSettingsFile))
            {
                try
                {
                    var serializer = new XmlLayoutSerializer(dockingManager1);
                    serializer.LayoutSerializationCallback += (s, args) =>
                    {
                        //args.Content = args.Content;
                        //args.Content = Application.Current.MainWindow.FindName(args.Model.ContentId);
                        if (args.Content is LayoutAnchorable)
                        {
                            LayoutAnchorable layout = args.Content as LayoutAnchorable;
                            if (layout.CanHide
                                && layout.IsHidden)
                            {
                                layout.Hide();
                            }
                        }
                    };

                    using (var stream = new StreamReader(_DockSettingsFile))
                        serializer.Deserialize(stream);
                }
                catch (Exception ex3)
                {
                    LogManager.Instance.WriteException(ex3);
                }
            }

            Left = l;
            Top = t;
            Width = w;
            Height = h;
            WindowState = winState;
        }

        /// <summary>
        /// 
        /// </summary>
        private void SaveSettings()
        {
            var serializer = new XmlLayoutSerializer(dockingManager1);
            using (var stream = new StreamWriter(_DockSettingsFile))
            {
                serializer.Serialize(stream);
            }

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.AddRootNode("FlowSimulator");
            rootNode.AddAttribute("version", "1");
            XmlNode winNode = xmlDoc.CreateElement("Window");
            rootNode.AppendChild(winNode);
            winNode.AddAttribute("version", "1");

            if (WindowState == WindowState.Minimized)
            {
                winNode.AddAttribute("windowState", Enum.GetName(typeof(WindowState), WindowState.Normal));
                winNode.AddAttribute("left", _LastLeft.ToString());
                winNode.AddAttribute("top", _LastTop.ToString());
                winNode.AddAttribute("width", _LastWidth.ToString());
                winNode.AddAttribute("height", _LastHeight.ToString());
            }
            else
            {
                winNode.AddAttribute("windowState", Enum.GetName(typeof(WindowState), WindowState));
                winNode.AddAttribute("left", Left.ToString());
                winNode.AddAttribute("top", Top.ToString());
                winNode.AddAttribute("width", Width.ToString());
                winNode.AddAttribute("height", Height.ToString());
            }

            //             _SessionControl.SaveSettings(rootNode);
            //             _MessageControl.SaveSettings(rootNode);
            //             _TaskControl.SaveSettings(rootNode);
            //             _ReportControl.SaveSettings(rootNode);
            //             _LogControl.SaveSettings(rootNode);
            //             _ScriptControl.SaveSettings(rootNode);

            xmlDoc.Save(_UserSettingsFile);
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetTitle()
        {
            Title = "Flow Simulator";
#if DEBUG
            Title += " - DEBUG";
#endif

            if (string.IsNullOrWhiteSpace(_FileOpened) == false)
            {
                Title += " - " + _FileOpened;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id_"></param>
        public void OpenScriptElement(ScriptElement el_)
        {
            var list = dockingManager1.Layout.Descendents().OfType<LayoutDocument>();

            foreach (LayoutDocument ld in list)
            {
                if (ld.Content is ScriptElementControl)
                {
                    ScriptElementControl s = ld.Content as ScriptElementControl;
                    if (s.Script.ID == el_.ID)
                    {
                        ld.IsSelected = true;
                        return;
                    }
                }
            }

            var firstDocumentPane = dockingManager1.Layout.Descendents().OfType<LayoutDocumentPane>().FirstOrDefault();
            if (firstDocumentPane != null)
            {
                LayoutDocument doc = new LayoutDocument
                {
                    Title = el_.Name,
                    Content = new ScriptElementControl(el_)
                };
                firstDocumentPane.Children.Add(doc);
                doc.IsSelected = true;
            }
        }
    }
}
