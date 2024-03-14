using BlazorWithRedux.Store.Counter.State;
using Fluxor;

namespace UnitTestsForBlazorWithRedux
{
    public class CounterTestsFixture
    {
        public IDispatcher dispatcher { get; set; }
        public UndoableCounterState state { get; set; }

        public CounterTestsFixture()
        {
            // Create or configure mock instances here
            dispatcher = new Dispatcher();
            state = new() { Present = new CounterState { Count = 0 } };
        }
    }
}
