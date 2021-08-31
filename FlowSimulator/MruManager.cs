using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace FlowSimulator
{
    using StringList = List<String>;
    using StringEnumerator = IEnumerator<String>;

    /// <summary>
    /// MRU manager - manages Most Recently Used Files list
    /// for Windows Form application.
    /// </summary>
    public class MruManager
    {
        // Event raised when user selects file from MRU list
        public event MruFileOpenEventHandler MruOpenEvent;

        private Window _ownerForm;                 // owner form

        private MenuItem _menuItemMru;           // Recent Files menu item
        private MenuItem _menuItemParent;        // Recent Files menu item parent

        private string _registryPath;            // Registry path to keep MRU list

        private int _maxNumberOfFiles = 10;      // maximum number of files in MRU list

        private int _maxDisplayLength = 40;      // maximum length of file name for display

        private readonly StringList _mruList;              // MRU list (file names)

        private const string RegEntryName = "file";  // entry name to keep MRU (file0, file1...)

        [DllImport("shlwapi.dll", CharSet = CharSet.Auto)]
        private static extern bool PathCompactPathEx(
            StringBuilder pszOut,
            string pszPath,
            int cchMax,
            int reserved);

        public MruManager()
        {
            _mruList = new StringList();
        }

        /// <summary>
		/// Gets the first file name in the list, otherwise return null
		/// </summary>
		public string GetFirstFileName => _mruList.Count == 0 ? null : _mruList[0];

        /// <summary>
        /// Maximum length of displayed file name in menu (default is 40).
        /// 
        /// Set this property to change default value (optional).
        /// </summary>
        public int MaxDisplayNameLength
        {
            set
            {
                _maxDisplayLength = value;

                if (_maxDisplayLength < 10)
                    _maxDisplayLength = 10;
            }

            get => _maxDisplayLength;
        }

        /// <summary>
        /// Maximum length of MRU list (default is 10).
        /// 
        /// Set this property to change default value (optional).
        /// </summary>
        public int MaxMruLength
        {
            set
            {
                _maxNumberOfFiles = value;

                if (_maxNumberOfFiles < 1)
                    _maxNumberOfFiles = 1;

                if (_mruList.Count > _maxNumberOfFiles)
                    _mruList.RemoveRange(_maxNumberOfFiles - 1, _mruList.Count - _maxNumberOfFiles);
            }

            get => _maxNumberOfFiles;
        }

        /// <summary>
        /// Set current directory.
        /// 
        /// Default value is program current directory which is set when
        /// Initialize function is called.
        /// 
        /// Set this property to change default value (optional)
        /// after call to Initialize.
        /// </summary>
        public string CurrentDir { set; get; }

        /// <summary>
        /// Initialization. Call this function in form Load handler.
        /// </summary>
        /// <param name="owner">Owner form</param>
        /// <param name="mruItem">Recent Files menu item</param>
        /// <param name="mruItemParent">parent menu item</param>
        /// <param name="regPath">Registry Path to keep MRU list</param>
        public void Initialize(Window owner, MenuItem mruItem, MenuItem mruItemParent, string regPath)
        {
            // keep reference to owner form
            _ownerForm = owner;

            // keep reference to MRU menu item
            _menuItemMru = mruItem;

            // keep reference to MRU menu item parent
            _menuItemParent = mruItemParent;


            // keep Registry path adding MRU key to it
            _registryPath = regPath;
            if (_registryPath.EndsWith("\\"))
                _registryPath += "MRU";
            else
                _registryPath += "\\MRU";


            // keep current directory in the time of initialization
            CurrentDir = Directory.GetCurrentDirectory();

            // subscribe to MRU parent Popup event
            _menuItemParent.SubmenuOpened += OnMRUParentPopup;

            // subscribe to owner form Closing event
            _ownerForm.Closing += OnOwnerClosing;

            // load MRU list from Registry
            LoadMRU();
        }

        /// <summary>
        /// Add file name to MRU list.
        /// Call this function when file is opened successfully.
        /// If file already exists in the list, it is moved to the first place.
        /// </summary>
        /// <param name="file">File Name</param>
        public void Add(string file)
        {
            Remove(file);

            // if array has maximum length, remove last element
            if (_mruList.Count == _maxNumberOfFiles)
                _mruList.RemoveAt(_maxNumberOfFiles - 1);

            // add new file name to the start of array
            _mruList.Insert(0, file);
        }

        /// <summary>
        /// Remove file name from MRU list.
        /// Call this function when File - Open operation failed.
        /// </summary>
        /// <param name="file">File Name</param>
        public void Remove(string file)
        {
            int i = 0;

            StringEnumerator myEnumerator = _mruList.GetEnumerator();

            while (myEnumerator.MoveNext())
            {
                if (myEnumerator.Current == file)
                {
                    _mruList.RemoveAt(i);
                    return;
                }

                i++;
            }
        }

        /// <summary>
        /// Update MRU list when MRU menu item parent is opened
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMRUParentPopup(object sender, RoutedEventArgs e)
        {
            // remove all childs
            //if (_menuItemMru.IsParent)
            //{
            _menuItemMru.Items.Clear();
            //}

            // Disable menu item if MRU list is empty
            if (_mruList.Count == 0)
            {
                _menuItemMru.IsEnabled = false;
                return;
            }

            // enable menu item and add child items
            _menuItemMru.IsEnabled = true;

            MenuItem item;

            StringEnumerator myEnumerator = _mruList.GetEnumerator();
            int i = 0;

            while (myEnumerator.MoveNext())
            {
                item = new MenuItem
                {
                    Header = GetDisplayName(myEnumerator.Current),
                    Tag = i++
                };

                // subscribe to item's Click event
                item.Click += OnMRUClicked;

                _menuItemMru.Items.Add(item);
            }
        }

        /// <summary>
        /// MRU menu item is clicked - call owner's OpenMRUFile function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMRUClicked(object sender, EventArgs e)
        {
            string s;

            // cast sender object to MenuItem
            MenuItem item = (MenuItem)sender;

            if (item != null)
            {
                // Get file name from list using item index
                s = _mruList[(int)item.Tag];

                // Raise event to owner and pass file name.
                // Owner should handle this event and open file.
                if (s.Length > 0)
                {
                    MruOpenEvent?.Invoke(this, new MruFileOpenEventArgs(s));
                }
            }
        }

        /// <summary>
        /// Save MRU list in Registry when owner form is closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOwnerClosing(object sender, CancelEventArgs e)
        {
            int i, n;

            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(_registryPath);

                if (key != null)
                {
                    n = _mruList.Count;

                    for (i = 0; i < _maxNumberOfFiles; i++)
                    {
                        key.DeleteValue(RegEntryName +
                            i.ToString(CultureInfo.InvariantCulture), false);
                    }

                    for (i = 0; i < n; i++)
                    {
                        key.SetValue(RegEntryName +
                            i.ToString(CultureInfo.InvariantCulture), _mruList[i]);
                    }
                }

            }
            catch (ArgumentNullException ex) { HandleWriteError(ex); }
            catch (SecurityException ex) { HandleWriteError(ex); }
            catch (ArgumentException ex) { HandleWriteError(ex); }
            catch (ObjectDisposedException ex) { HandleWriteError(ex); }
            catch (UnauthorizedAccessException ex) { HandleWriteError(ex); }
        }


        /// <summary>
        /// Load MRU list from Registry.
        /// Called from Initialize.
        /// </summary>
        private void LoadMRU()
        {
            try
            {
                _mruList.Clear();

                RegistryKey key = Registry.CurrentUser.OpenSubKey(_registryPath);

                if (key != null)
                {
                    for (int i = 0; i < _maxNumberOfFiles; i++)
                    {
                        var sKey = RegEntryName + i.ToString(CultureInfo.InvariantCulture);
                        var s = (string)key.GetValue(sKey, "");

                        if (s.Length == 0)
                            break;

                        _mruList.Add(s);
                    }
                }
            }
            catch (ArgumentNullException ex) { HandleReadError(ex); }
            catch (SecurityException ex) { HandleReadError(ex); }
            catch (ArgumentException ex) { HandleReadError(ex); }
            catch (ObjectDisposedException ex) { HandleReadError(ex); }
            catch (UnauthorizedAccessException ex) { HandleReadError(ex); }
        }

        /// <summary>
        /// Handle error from OnOwnerClosing function
        /// </summary>
        /// <param name="ex"></param>
        private void HandleReadError(Exception ex)
        {
            Trace.WriteLine("Loading MRU from Registry failed: " + ex.Message);
        }

        /// <summary>
        /// Handle error from OnOwnerClosing function
        /// </summary>
        /// <param name="ex"></param>
        private void HandleWriteError(Exception ex)
        {
            Trace.WriteLine("Saving MRU to Registry failed: " + ex.Message);
        }


        /// <summary>
        /// Get display file name from full name.
        /// </summary>
        /// <param name="fullName">Full file name</param>
        /// <returns>Short display name</returns>
        private string GetDisplayName(string fullName)
        {
            // if file is in current directory, show only file name
            FileInfo fileInfo = new FileInfo(fullName);

            if (fileInfo.DirectoryName == CurrentDir)
                return GetShortDisplayName(fileInfo.Name, _maxDisplayLength);

            return GetShortDisplayName(fullName, _maxDisplayLength);
        }

        /// <summary>
        /// Truncate a path to fit within a certain number of characters 
        /// by replacing path components with ellipses.
        /// 
        /// This solution is provided by CodeProject and GotDotNet C# expert
        /// Richard Deeming.
        /// 
        /// </summary>
        /// <param name="longName">Long file name</param>
        /// <param name="maxLen">Maximum length</param>
        /// <returns>Truncated file name</returns>
        private string GetShortDisplayName(string longName, int maxLen)
        {
            StringBuilder pszOut = new StringBuilder(maxLen + maxLen + 2);  // for safety

            if (PathCompactPathEx(pszOut, longName, maxLen, 0))
            {
                return pszOut.ToString();
            }

            return longName;
        }
    }

    public delegate void MruFileOpenEventHandler(object sender, MruFileOpenEventArgs e);

    public class MruFileOpenEventArgs : EventArgs
    {
        public MruFileOpenEventArgs(string fileName)
        {
            FileName = fileName;
        }

        public string FileName { get; }
    }
}

/*******************************************************************************

NOTE: This class works with new Visual Studio 2005 MenuStrip class.
If owner form has old-style MainMenu, use previous MruManager version.

Using:

1) Add menu item Recent Files (or any name you want) to main application menu.
   This item is used by MruManager as popup menu for MRU list.

2) Write function which opens file selected from MRU:

     private void mruManager_MruOpenEvent(object sender, MruFileOpenEventArgs e)
     {
         // open file e.FileName
     }

3) Add MruManager member to the form class and initialize it:

     private MruManager mruManager;

   Initialize it in the form initialization code:
   
         mruManager = new MruManager();
         mruManager.Initialize(
             this,                              // owner form
             mnuFileMRU,                        // Recent Files menu item (ToolStripMenuItem class)
             mruItemParent,                     // Recent Files menu item parent (ToolStripMenuItem class)
             "Software\\MyCompany\\MyProgram"); // Registry path to keep MRU list

        mruManager.MruOpenEvent += this.mruManager_MruOpenEvent;

        // Optional. Call these functions to change default values:

        mruManager.CurrentDir = ".....";           // default is current directory
        mruManager.MaxMruLength = ...;             // default is 10
        mruMamager.MaxDisplayNameLength = ...;     // default is 40

     NOTES:
     - If Registry path is, for example, "Software\MyCompany\MyProgram",
       MRU list is kept in
       HKEY_CURRENT_USER\Software\MyCompany\MyProgram\MRU Registry entry.

     - CurrentDir is used to show file names in the menu. If file is in
       this directory, only file name is shown.

4) Call MruManager Add and Remove functions when necessary:

       mruManager.Add(fileName);          // when file is successfully opened

       mruManager.Remove(fileName);       // when Open File operation failed

*******************************************************************************/

// Implementation details:
//
// MruManager loads MRU list from Registry in Initialize function.
// List is saved in Registry when owner form is closed.
//
// MRU list in the menu is updated when parent menu is poped-up.
//
// Owner form OnMRUFileOpen function is called when user selects file
// from MRU list.