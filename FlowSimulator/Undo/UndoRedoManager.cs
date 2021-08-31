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
        public event EventHandler UndoRedoCommandListChanged;
        public event EventHandler UndoRedoCommandExecuted;

        readonly Stack<IUndoCommand> _Undo = new Stack<IUndoCommand>();
        readonly Stack<IUndoCommand> _Redo = new Stack<IUndoCommand>();

        /// <summary>
        /// Used to not add/delete action when undoing or redoing
        /// </summary>
        bool _IsProcessing;

        /// <summary>
        /// Gets if can undo
        /// </summary>
        public bool CanUndo => _Undo.Count == 0 ? false : true;

        /// <summary>
        /// Gets if can redo
        /// </summary>
        public bool CanRedo => _Redo.Count == 0 ? false : true;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command_"></param>
        /// <param name="arg_"></param>
        public void Add(IUndoCommand command_)
        {
            if (_IsProcessing)
            {
                LogManager.Instance.WriteLine(LogVerbosity.Trace, "UndoRedo : can't add because processing");
                return;
            }

            _Undo.Push(command_);
            _Redo.Clear();

            LogManager.Instance.WriteLine(LogVerbosity.Trace, "UndoRedo Add({0}) : history list: {1} item(s)",
                command_.ToString(), _Undo.Count);

            OnPropertyChanged("CanUndo");
            OnPropertyChanged("CanRedo");

            UndoRedoCommandListChanged?.Invoke(null, EventArgs.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command_"></param>
        /// <param name="arg_"></param>
        public void Clear()
        {
            _Undo.Clear();
            _Redo.Clear();

            LogManager.Instance.WriteLine(LogVerbosity.Trace, "UndoRedo Clear()");

            OnPropertyChanged("CanUndo");
            OnPropertyChanged("CanRedo");

            UndoRedoCommandListChanged?.Invoke(null, EventArgs.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Undo()
        {
            if (CanUndo)
            {
                _IsProcessing = true;

                try
                {
                    IUndoCommand command = _Undo.Pop();

                    LogManager.Instance.WriteLine(LogVerbosity.Trace, "UndoRedo Undo({0}) : history list: {1} item(s)",
                        command.ToString(), _Undo.Count);

                    command.Undo();
                    _Redo.Push(command);

                    OnPropertyChanged("CanUndo");
                    OnPropertyChanged("CanRedo");

                    UndoRedoCommandExecuted?.Invoke(null, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    _IsProcessing = false;
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
                _IsProcessing = true;

                try
                {
                    IUndoCommand command = _Redo.Pop();

                    LogManager.Instance.WriteLine(LogVerbosity.Trace, "UndoRedo Redo({0}) : history list: {1} item(s)",
                        command.ToString(), _Undo.Count);

                    command.Redo();
                    _Undo.Push(command);

                    OnPropertyChanged("CanUndo");
                    OnPropertyChanged("CanRedo");

                    UndoRedoCommandExecuted?.Invoke(null, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    _IsProcessing = false;
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
        /// <param name="nu_"></param>
        public void RemoveLastNCommands(int nu_)
        {
            for (int i = 0; i < nu_; i++)
            {
                if (CanUndo)
                {
                    _Undo.Pop();
                }
                else
                {
                    break;
                }
            }

            LogManager.Instance.WriteLine(LogVerbosity.Trace, "UndoRedo RemoveLastNCommands({0}) : history list: {1} item(s)",
                    nu_, _Undo.Count);

            OnPropertyChanged("CanUndo");
            OnPropertyChanged("CanRedo");

            UndoRedoCommandListChanged?.Invoke(null, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Event raised to indicate that a property value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
