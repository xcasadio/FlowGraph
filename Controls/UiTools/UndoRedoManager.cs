using System.ComponentModel;
using Logger;

namespace UiTools
{
    public class UndoRedoManager : INotifyPropertyChanged
    {
        private readonly ILogManager _logManager;
        public event EventHandler UndoRedoCommandListChanged;
        public event EventHandler UndoRedoCommandExecuted;

        readonly Stack<IUndoCommand> _undo = new Stack<IUndoCommand>();
        readonly Stack<IUndoCommand> _redo = new Stack<IUndoCommand>();

        bool _isProcessing;

        public bool CanUndo => _undo.Count != 0;
        public bool CanRedo => _redo.Count != 0;

        public UndoRedoManager(ILogManager logManager)
        {
            _logManager = logManager;
        }

        public void Add(IUndoCommand command)
        {
            if (_isProcessing)
            {
                _logManager.WriteLine(LogVerbosity.Trace, "UndoRedo : can't add because processing");
                return;
            }

            _undo.Push(command);
            _redo.Clear();

            _logManager.WriteLine(LogVerbosity.Trace, "UndoRedo Add({0}) : history list: {1} item(s)",
                command.ToString(), _undo.Count);

            OnPropertyChanged("CanUndo");
            OnPropertyChanged("CanRedo");

            UndoRedoCommandListChanged?.Invoke(null, EventArgs.Empty);
        }

        public void Clear()
        {
            _undo.Clear();
            _redo.Clear();

            _logManager.WriteLine(LogVerbosity.Trace, "UndoRedo Clear()");

            OnPropertyChanged("CanUndo");
            OnPropertyChanged("CanRedo");

            UndoRedoCommandListChanged?.Invoke(null, EventArgs.Empty);
        }

        public void Undo()
        {
            if (CanUndo)
            {
                _isProcessing = true;

                try
                {
                    IUndoCommand command = _undo.Pop();

                    _logManager.WriteLine(LogVerbosity.Trace, "UndoRedo Undo({0}) : history list: {1} item(s)",
                        command.ToString(), _undo.Count);

                    command.Undo();
                    _redo.Push(command);

                    OnPropertyChanged("CanUndo");
                    OnPropertyChanged("CanRedo");

                    UndoRedoCommandExecuted?.Invoke(null, EventArgs.Empty);
                }
                finally
                {
                    _isProcessing = false;
                }
            }
        }

        public void Redo()
        {
            if (CanRedo)
            {
                _isProcessing = true;

                try
                {
                    IUndoCommand command = _redo.Pop();

                    _logManager.WriteLine(LogVerbosity.Trace, "UndoRedo Redo({0}) : history list: {1} item(s)",
                        command.ToString(), _undo.Count);

                    command.Redo();
                    _undo.Push(command);

                    OnPropertyChanged("CanUndo");
                    OnPropertyChanged("CanRedo");

                    UndoRedoCommandExecuted?.Invoke(null, EventArgs.Empty);
                }
                finally
                {
                    _isProcessing = false;
                }
            }
        }

        public void RemoveLastCommand()
        {
            RemoveLastNCommands(0);
        }

        public void RemoveLastNCommands(int nu)
        {
            for (int i = 0; i < nu; i++)
            {
                if (CanUndo)
                {
                    _undo.Pop();
                }
                else
                {
                    break;
                }
            }

            _logManager.WriteLine(LogVerbosity.Trace, "UndoRedo RemoveLastNCommands({0}) : history list: {1} item(s)",
                    nu, _undo.Count);

            OnPropertyChanged("CanUndo");
            OnPropertyChanged("CanRedo");

            UndoRedoCommandListChanged?.Invoke(null, EventArgs.Empty);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
