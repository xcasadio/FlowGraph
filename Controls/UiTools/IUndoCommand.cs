namespace UiTools
{
    public interface IUndoCommand
    {
        void Redo();
        void Undo();
    }
}
