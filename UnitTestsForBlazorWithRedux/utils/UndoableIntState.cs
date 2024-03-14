using Fluxor.Undo;

namespace UnitTestsForBlazorWithRedux.utils
{
    public sealed record UndoableIntState : Undoable<UndoableIntState, int>;
}
