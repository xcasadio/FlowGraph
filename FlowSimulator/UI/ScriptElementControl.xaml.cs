using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using FlowGraphBase.Script;
using FlowSimulator.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Indentation.CSharp;
using ICSharpCode.AvalonEdit.Search;

namespace FlowSimulator.UI
{
    /// <summary>
    /// Interaction logic for ScriptElementControl.xaml
    /// </summary>
    public partial class ScriptElementControl : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public ScriptElement Script
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public ScriptElementControl(ScriptElement el_)
        {
            DataContext = el_;
            InitializeComponent();
            Script = el_;

            textEditor.Text = Script.ScriptSourceCode;

            SetFoldingStrategy();
            textEditor.TextArea.TextEntering += textEditor_TextArea_TextEntering;
            textEditor.TextArea.TextEntered += textEditor_TextArea_TextEntered;

            DispatcherTimer foldingUpdateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };
            foldingUpdateTimer.Tick += foldingUpdateTimer_Tick;
            foldingUpdateTimer.Start();

            textEditor.TextArea.DefaultInputHandler.NestedInputHandlers.Add(
                new SearchInputHandler(textEditor.TextArea));
        }

        FoldingManager foldingManager;
        AbstractFoldingStrategy foldingStrategy;

        /// <summary>
        /// 
        /// </summary>
        void SetFoldingStrategy()
        {
            if (textEditor.SyntaxHighlighting == null)
            {
                foldingStrategy = null;
            }
            else
            {
//                 switch (textEditor.SyntaxHighlighting.Name)
//                 {
//                     case "XML":
//                         foldingStrategy = new XmlFoldingStrategy();
//                         textEditor.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.DefaultIndentationStrategy();
//                         break;
//                     case "C#":
//                     case "C++":
//                     case "PHP":
//                     case "Java":
                textEditor.TextArea.IndentationStrategy = new CSharpIndentationStrategy(textEditor.Options);
                foldingStrategy = new BraceFoldingStrategy();
//                         break;
//                     default:
//                         textEditor.TextArea.IndentationStrategy = new ICSharpCode.AvalonEdit.Indentation.DefaultIndentationStrategy();
//                         foldingStrategy = null;
//                          break;
//                 }
            }
            if (foldingStrategy != null)
            {
                if (foldingManager == null)
                    foldingManager = FoldingManager.Install(textEditor.TextArea);
                foldingStrategy.UpdateFoldings(foldingManager, textEditor.Document);
            }
            else
            {
                if (foldingManager != null)
                {
                    FoldingManager.Uninstall(foldingManager);
                    foldingManager = null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void foldingUpdateTimer_Tick(object sender, EventArgs e)
        {
            foldingStrategy?.UpdateFoldings(foldingManager, textEditor.Document);
        }

        CompletionWindow completionWindow;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void textEditor_TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == ".")
            {
                // open code completion after the user has pressed dot:
                completionWindow = new CompletionWindow(textEditor.TextArea);
                // provide AvalonEdit with the data:
                IList<ICompletionData> data = completionWindow.CompletionList.CompletionData;
                //                 data.Add(new MyCompletionData("Item1"));
                //                 data.Add(new MyCompletionData("Item2"));
                //                 data.Add(new MyCompletionData("Item3"));
                //                 data.Add(new MyCompletionData("Another item"));
                completionWindow.Show();
                completionWindow.Closed += delegate
                {
                    completionWindow = null;
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void textEditor_TextArea_TextEntering(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Length > 0 && completionWindow != null)
            {
                if (!char.IsLetterOrDigit(e.Text[0]))
                {
                    // Whenever a non-letter is typed while the completion window is open,
                    // insert the currently selected element.
                    completionWindow.CompletionList.RequestInsertion(e);
                }
            }
            // do not set e.Handled=true - we still want to insert the character that was typed
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textEditor_TextChanged(object sender, EventArgs e)
        {
            Script.ScriptSourceCode = textEditor.Text;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonBuild_Click(object sender, RoutedEventArgs e)
        {
            if (Script.CompilScript() == false)
            {

            }
        }
    }
}
