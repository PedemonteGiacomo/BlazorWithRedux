using Fluxor;
using Fluxor.Undo;
using BlazorWithRedux.Store.Counter.Reducers;
using BlazorWithRedux.Store.Counter.Actions;
using System.Diagnostics.CodeAnalysis;

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

    public sealed record UndoableCounterState : Undoable<UndoableCounterState, CounterState>
    {

        [SetsRequiredMembers]
        public UndoableCounterState() => Present = new CounterState { Count = 0 };
        // added this constructor to initialize the state and avoiding making the new instance
        // of the state in the test be more heavy since we don't need anymore
        // to set the Present property that in the Undoable library is required
    }
}
