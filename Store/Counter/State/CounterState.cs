using Fluxor;
using Fluxor.Undo;

namespace BlazorWithRedux.Store.Counter.State
{
    public sealed record CounterState
    {
        public int Count { get; init; }

        public int DoubleCount => Count * 2;

        public bool IsEven => Count % 2 == 0;

        public bool IsOdd => !IsEven;

        public bool IsNegative => Count < 0;

        public bool IsPositive => Count > 0;

        public bool IsZero => Count == 0;
    }
    public sealed record UndoableCounterState : Undoable<UndoableCounterState, CounterState>;
}
