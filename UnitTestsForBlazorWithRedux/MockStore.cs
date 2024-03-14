using BlazorWithRedux.Store.Counter.State;
using Fluxor;

namespace UnitTestsForBlazorWithRedux
{
    public class MockStore : IDispatcher, IState<UndoableCounterState>
    {
        private UndoableCounterState _state = new() { Present = new CounterState { Count = 0 } };

        public UndoableCounterState Value => GetState<UndoableCounterState>();

        public void Dispatch(object action)
        {
            ActionDispatched?.Invoke(this, new ActionDispatchedEventArgs(action));
        }

        public TState GetState<TState>() => (TState)(object)_state;

        public event EventHandler StateChanged;

        public event EventHandler<ActionDispatchedEventArgs> ActionDispatched;
    }

}
