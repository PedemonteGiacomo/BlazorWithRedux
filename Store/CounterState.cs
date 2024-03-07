using Fluxor;
using Fluxor.Undo;

namespace BlazorWithRedux.Store
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

    // The UndoableCounterState class is used to manage the state of the counter in the Redux store.
    // using the featureState attribute, we can specify the name of the feature and the method that will create the initial state
    //[FeatureState(Name = "Counter", CreateInitialStateMethodName = nameof(CreateInitialState))]
    //public sealed record UndoableCounterState : Undoable<UndoableCounterState, CounterState>
    //{
    //    public static UndoableCounterState CreateInitialState()
    //        => new() { Present = new CounterState { Count = 0 } };
    //}

    // The UndoableCounterState class is used to manage the state of the counter in the Redux store.
    // this is the feature class that will be used to manage the state of the counter in the Redux store
    public sealed record UndoableCounterState : Undoable<UndoableCounterState, CounterState>;

    public sealed class UndoableCounterFeature : Feature<UndoableCounterState>
    {
        public override string GetName() => nameof(CounterState);

        protected override UndoableCounterState GetInitialState() => new() { Present = new CounterState { Count = 0 } };
    }
}
