namespace FlowSimulator.Undo
{
    public interface IUndoCommand
    {
        void Redo();
        void Undo();
    }
}
