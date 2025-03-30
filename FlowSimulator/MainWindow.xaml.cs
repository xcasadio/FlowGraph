using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml;
using CustomNode;
using FlowGraph;
using FlowGraph.Logger;
using FlowGraph.Process;
using FlowGraphUI;
using Logger;
using Xceed.Wpf.AvalonDock.Layout;
using Xceed.Wpf.AvalonDock.Layout.Serialization;
using MenuItem = System.Windows.Controls.MenuItem;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace FlowSimulator
{
    public partial class MainWindow : Window
    {
        private readonly string _userSettingsFile = "userSettings.xml";
        private readonly string _dockSettingsFile = "dockSettings.xml";

        private readonly MruManager _mruManager = new();
        private const string RegistryPath = "Software\\Casaprod\\FlowSimulator";

        private string _fileOpened = "";
        private double _lastLeft, _lastTop, _lastWidth, _lastHeight;

        private readonly FlowGraphViewModel _flowGraphViewModel;
        private readonly string _projectFileFilter = "Json files (*.json)|*.json";

        internal static MainWindow Instance
        {
            get;
            private set;
        }

        internal DetailsControl DetailsControl => detailsControl;

        public MainWindow()
        {
            Instance = this;

            InitializeComponent();

            _flowGraphViewModel = new FlowGraphViewModel(new FlowGraphManager());
            DataContext = _flowGraphViewModel;
            
            SetTitle();

            var ver = Assembly.GetExecutingAssembly().GetName().Version!;
            statusLabelVersion.Content = "v" + ver;
            LogManager.Instance.WriteLine(LogVerbosity.Info, "FlowSimulator - v{0} started", ver);

            Loaded += OnLoaded;
            Closed += OnClosed;
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _mruManager.Initialize(this, menuItemRecentFiles, menuItemFile, RegistryPath);
                _mruManager.MruOpenEvent += delegate (object s, MruFileOpenEventArgs openEvent)
                {
                    SaveChangesOnDemand();
                    LoadFile(openEvent.FileName);
                };

                //after this call AppDomain.CurrentDomain.GetAssemblies() will contains CustomNode.dll
                Assembly.LoadFrom("CustomNode.dll"); 
                NodeRegister.RegisterAssemblies(AppDomain.CurrentDomain.GetAssemblies());

                LoadSettings();

                //                 TaskLauncher.TaskLaunch += new EventHandler<EventArg1Param<Task>>(OnTaskLaunch);
                //                 TaskLauncher.TaskFinish += new EventHandler<EventArg1Param<Task>>(OnTaskFinish);
                //                 TaskLauncher.TaskMessageSent += new EventHandler<EventArg2Params<int, bool>>(OnTaskMessageSent);
                //                 TaskLauncher.TaskCountChanged += new EventHandler<EventArg1Param<int>>(OnTaskCountChanged);
                //                 _ReportControl.StartReporting += new EventHandler(OnStartReporting);
                //                 _ReportControl.StopReporting += new EventHandler(OnStopReporting);
                // 
                if (string.IsNullOrWhiteSpace(_mruManager.GetFirstFileName) == false)
                {
                    //LoadFile(_mruManager.GetFirstFileName);
                }

                _lastLeft = Left;
                _lastTop = Top;
                _lastWidth = Width;
                _lastHeight = Height;

                ProcessLauncher.Instance.StartLoop();
            }
            catch (System.Exception ex)
            {
                LogManager.Instance.WriteException(ex);
            }
        }

        private void OnClosed(object? sender, EventArgs e)
        {
            ProcessLauncher.Instance.StopLoop();
            LogManager.Instance.WriteLine(LogVerbosity.Info, "Closed by user");

            try
            {
                SaveSettings();
                //SaveChangesOnDemand();
            }
            catch (System.Exception ex)
            {
                LogManager.Instance.WriteException(ex);
            }
        }

        private void Launch_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _flowGraphViewModel.SequenceViewModel.CreateSequence();
            ProcessLauncher.Instance.LaunchSequence(_flowGraphViewModel.SequenceViewModel.SequenceBase, typeof(EventTestStartedNode), 0, "test");
        }

        private void Resume_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ProcessLauncher.Instance.Resume();
        }

        private void NextStep_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ProcessLauncher.Instance.NextStep();
        }

        private void Pause_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ProcessLauncher.Instance.Pause();
        }

        private void Stop_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ProcessLauncher.Instance.Stop();
        }

        private void NewCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NewProject();
        }

        private void OpenCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenProject();
        }

        private void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveProject();
        }

        private void SaveAsCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveAsProject();
        }

        private void ExitCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Exit();
        }

        private void menuItemWindows_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            menuItemWindows.Items.Clear();
            var list = new SortedList<string, MenuItem>();

            foreach (var content in dockingManager1.Layout.Descendents().OfType<LayoutContent>())
            {
                var item = new MenuItem
                {
                    Header = content.Title,
                    IsChecked = content is LayoutDocument document ?
                        document.IsVisible : content is LayoutAnchorable anchorable ?
                            anchorable.IsVisible : false
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

        private void MenuItemLayout_Click(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuItem;
            var l = item.Tag as LayoutAnchorable;
            l.IsVisible = !l.IsVisible;
            item.IsChecked = l.IsVisible;
        }

        private void MenuItemWorkspaceSave_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
        }

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

        private void MenuIte_HelpClick(object sender, RoutedEventArgs e)
        {
            var msgBox = new MessageBox();
            windowContainer.Children.Add(msgBox);

            var img = new Image
            {
                Source = new BitmapImage(new Uri(
                        "pack://application:,,,/FlowSimulator;component/Resources/Mattahan-Ultrabuuf-Comics-War-Machine.ico"))
            };
            msgBox.ImageSource = img.Source;

            msgBox.ShowMessageBox(
                "Flow Simulator version " + Assembly.GetExecutingAssembly().GetName().Version + Environment.NewLine +
                "Developed by Xavier Casadio",
                "Information",
                MessageBoxButton.OK);
        }

        private void Clear()
        {
            ProjectManager.Clear();

            var list = dockingManager1.Layout.Descendents().OfType<LayoutDocument>().ToList();

            foreach (var ld in list)
            {
                ld.Close();
            }
        }

        private void NewProject()
        {
            SaveChangesOnDemand();

            LogManager.Instance.WriteLine(LogVerbosity.Info, "New project");
            LogManager.Instance.NbErrors = 0;

            _fileOpened = "";

            Clear();
            SetTitle();
        }

        private void OpenProject()
        {
            SaveChangesOnDemand();

            using var form = new OpenFileDialog
            {
                Filter = _projectFileFilter,
                Multiselect = false
            };

            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LoadFile(form.FileName);
            }
        }

        private void SaveProject()
        {
            if (string.IsNullOrWhiteSpace(_fileOpened))
            {
                SaveAsProject();
            }
            else
            {
                SaveFile(_fileOpened);
            }
        }

        private void SaveAsProject()
        {
            using var form = new SaveFileDialog
            {
                Filter = _projectFileFilter
            };

            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SaveFile(form.FileName);
            }
        }

        private void Exit()
        {
            Close();
        }

        private void workSpaceSaveToolStripMenuIte_Click(object sender, EventArgs e)
        {
            SaveSettings();
            LogManager.Instance.WriteLine(LogVerbosity.Debug, "Workspace saved");
        }

        private void LoadFile(string fileName, bool addToMru = true)
        {
            if (File.Exists(fileName))
            {
                Clear();

                if (ProjectManager.OpenFile(fileName, _flowGraphViewModel.FlowGraphManager))
                {
                    _flowGraphViewModel.Initialize();
                    _fileOpened = fileName;
                    SetTitle();

                    if (addToMru)
                    {
                        _mruManager.Add(fileName);
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show(this,
                        "Can't load the file '" + fileName + "'. Please check the log.",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    _mruManager.Remove(fileName);
                }
            }
            else
            {
                System.Windows.MessageBox.Show(this,
                    "The file '" + fileName + "' doesn't exist.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _mruManager.Remove(fileName);
            }
        }

        private void SaveFile(string fileName)
        {
            if (ProjectManager.SaveFile(fileName, _flowGraphViewModel.FlowGraphManager))
            {
                _mruManager.Add(fileName);
                _fileOpened = fileName;
            }
        }

        private void SaveChangesOnDemand()
        {
            Debugger.Break();
            /*if (_flowGraphManagerViewModel.IsChanges())
            {
                if (System.Windows.MessageBox.Show(this, "Save changes ?", "Save ?",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    SaveProject();
                }
            }*/
        }

        private void LoadSettings()
        {
            double l = Left, t = Top, w = Width, h = Height;
            var winState = WindowState;

            if (File.Exists(_userSettingsFile))
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(_userSettingsFile);

                var winNode = xmlDoc.SelectSingleNode("FlowSimulator/Window");

                var version = int.Parse(winNode.Attributes["version"].Value);

                l = int.Parse(winNode.Attributes["left"].Value);
                t = int.Parse(winNode.Attributes["top"].Value);
                w = int.Parse(winNode.Attributes["width"].Value);
                h = int.Parse(winNode.Attributes["height"].Value);
                winState = (WindowState)Enum.Parse(typeof(WindowState), winNode.Attributes["windowState"].Value);

                var rootNode = xmlDoc.SelectSingleNode("FlowSimulator");

                try
                {
                    //                         _SessionControl.LoadSettings(rootNode);
                    //                         _MessageControl.LoadSettings(rootNode);
                    //                         _TaskControl.LoadSettings(rootNode);
                    //                         _ReportControl.LoadSettings(rootNode);
                    //                         //_ScriptControl.LoadSettings(rootNode);
                    //                         _LogControl.LoadSettings(rootNode);
                }
                catch (System.Exception ex2)
                {
                    LogManager.Instance.WriteException(ex2);
                }
            }

            if (File.Exists(_dockSettingsFile))
            {
                try
                {
                    var serializer = new XmlLayoutSerializer(dockingManager1);
                    serializer.LayoutSerializationCallback += (s, args) =>
                    {
                        if (args.Model.Title == "Flow Graph List")
                        {
                            args.Content = flowGraphListContainer;
                        }
                        else if (args.Model.Title == "Details")
                        {
                            args.Content = detailsGrid;
                        }
                        else if (args.Model.Title == "Action Graph")
                        {
                            args.Content = containerFlowGraph;
                        }
                        else if (args.Model.Title == "Log")
                        {
                            args.Content = gridLog;
                        }

                        if (args.Content is LayoutAnchorable { CanHide: true, IsHidden: true } layout)
                        {
                            layout.Hide();
                        }
                    };

                    using var stream = new StreamReader(_dockSettingsFile);
                    serializer.Deserialize(stream);
                }
                catch (System.Exception ex3)
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

        private void SaveSettings()
        {
            var serializer = new XmlLayoutSerializer(dockingManager1);
            using (var stream = new StreamWriter(_dockSettingsFile))
            {
                serializer.Serialize(stream);
            }

            var xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.AddRootNode("FlowSimulator");
            rootNode.AddAttribute("version", "1");
            XmlNode winNode = xmlDoc.CreateElement("Window");
            rootNode.AppendChild(winNode);
            winNode.AddAttribute("version", "1");

            if (WindowState == WindowState.Minimized)
            {
                winNode.AddAttribute("windowState", Enum.GetName(WindowState.Normal));
                winNode.AddAttribute("left", _lastLeft.ToString(CultureInfo.InvariantCulture));
                winNode.AddAttribute("top", _lastTop.ToString(CultureInfo.InvariantCulture));
                winNode.AddAttribute("width", _lastWidth.ToString(CultureInfo.InvariantCulture));
                winNode.AddAttribute("height", _lastHeight.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                winNode.AddAttribute("windowState", Enum.GetName(WindowState));
                winNode.AddAttribute("left", Left.ToString(CultureInfo.InvariantCulture));
                winNode.AddAttribute("top", Top.ToString(CultureInfo.InvariantCulture));
                winNode.AddAttribute("width", Width.ToString(CultureInfo.InvariantCulture));
                winNode.AddAttribute("height", Height.ToString(CultureInfo.InvariantCulture));
            }

            //             _SessionControl.SaveSettings(rootNode);
            //             _MessageControl.SaveSettings(rootNode);
            //             _TaskControl.SaveSettings(rootNode);
            //             _ReportControl.SaveSettings(rootNode);
            //             _LogControl.SaveSettings(rootNode);
            //             _ScriptControl.SaveSettings(rootNode);

            xmlDoc.Save(_userSettingsFile);
        }

        private void SetTitle()
        {
            Title = "Flow Simulator";
#if DEBUG
            Title += " - DEBUG";
#endif

            if (string.IsNullOrWhiteSpace(_fileOpened) == false)
            {
                Title += " - " + _fileOpened;
            }
        }
    }
}
