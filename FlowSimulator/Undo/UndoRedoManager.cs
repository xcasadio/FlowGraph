using System;
using System.Collections.Generic;
using System.ComponentModel;
using FlowGraphBase.Logger;

namespace FlowSimulator.Undo
{
    /// <summary>
    /// 
    /// </summary>
    public class UndoRedoManager : INotifyPropertyChanged
    {
        #region Fields

        public event EventHandler UndoRedoCommandListChanged;
        public event EventHandler UndoRedoCommandExecuted;

        Stack<IUndoCommand> m_Undo = new Stack<IUndoCommand>();
        Stack<IUndoCommand> m_Redo = new Stack<IUndoCommand>();

        /// <summary>
        /// Used to not add/delete action when undoing or redoing
        /// </summary>
        bool m_IsProcessing = false;

        #endregion //Fields

        #region Properties

        /// <summary>
        /// Gets if can undo
        /// </summary>
        public bool CanUndo => m_Undo.Count == 0 ? false : true;

        /// <summary>
        /// Gets if can redo
        /// </summary>
        public bool CanRedo => m_Redo.Count == 0 ? false : true;

        #endregion //Properties

        #region Constructors

        #endregion //Constructors

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command_"></param>
        /// <param name="arg_"></param>
        public void Add(IUndoCommand command_)
        {
            if (m_IsProcessing)
            {
                LogManager.Instance.WriteLine(LogVerbosity.Trace, "UndoRedo : can't add because processing");
                return;
            }

            m_Undo.Push(command_);
            m_Redo.Clear();

            LogManager.Instance.WriteLine(LogVerbosity.Trace, "UndoRedo Add({0}) : history list: {1} item(s)",
                command_.ToString(), m_Undo.Count);

            OnPropertyChanged("CanUndo");
            OnPropertyChanged("CanRedo");

            if (UndoRedoCommandListChanged != null)
            {
                UndoRedoCommandListChanged(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command_"></param>
        /// <param name="arg_"></param>
        public void Clear()
        {
            m_Undo.Clear();
            m_Redo.Clear();

            LogManager.Instance.WriteLine(LogVerbosity.Trace, "UndoRedo Clear()");

            OnPropertyChanged("CanUndo");
            OnPropertyChanged("CanRedo");

            if (UndoRedoCommandListChanged != null)
            {
                UndoRedoCommandListChanged(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Undo()
        {
            if (CanUndo)
            {
                m_IsProcessing = true;

                try
                {
                    IUndoCommand command = m_Undo.Pop();

                    LogManager.Instance.WriteLine(LogVerbosity.Trace, "UndoRedo Undo({0}) : history list: {1} item(s)",
                        command.ToString(), m_Undo.Count);

                    command.Undo();
                    m_Redo.Push(command);

                    OnPropertyChanged("CanUndo");
                    OnPropertyChanged("CanRedo");

                    if (UndoRedoCommandExecuted != null)
                    {
                        UndoRedoCommandExecuted(null, EventArgs.Empty);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    m_IsProcessing = false;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Redo()
        {
            if (CanRedo)
            {
                m_IsProcessing = true;

                try
                {
                    IUndoCommand command = m_Redo.Pop();

                    LogManager.Instance.WriteLine(LogVerbosity.Trace, "UndoRedo Redo({0}) : history list: {1} item(s)",
                        command.ToString(), m_Undo.Count);

                    command.Redo();
                    m_Undo.Push(command);

                    OnPropertyChanged("CanUndo");
                    OnPropertyChanged("CanRedo");

                    if (UndoRedoCommandExecuted != null)
                    {
                        UndoRedoCommandExecuted(null, EventArgs.Empty);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    m_IsProcessing = false;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveLastCommand()
        {
            RemoveLastNCommands(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="num_"></param>
        public void RemoveLastNCommands(int num_)
        {
            for (int i = 0; i < num_; i++)
            {
                if (CanUndo)
                {
                    m_Undo.Pop();
                }
                else
                {
                    break;
                }
            }

            LogManager.Instance.WriteLine(LogVerbosity.Trace, "UndoRedo RemoveLastNCommands({0}) : history list: {1} item(s)",
                    num_, m_Undo.Count);

            OnPropertyChanged("CanUndo");
            OnPropertyChanged("CanRedo");

            if (UndoRedoCommandListChanged != null)
            {
                UndoRedoCommandListChanged(null, EventArgs.Empty);
            }
        }

        #region INotifyPropertyChanged

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Event raised to indicate that a property value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion // INotifyPropertyChanged

        #endregion //Methods
    }
}
