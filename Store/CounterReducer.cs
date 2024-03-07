// Purpose: Contains the CounterReducer class which is used to manage the state of the counter in the Redux store.
// The CounterReducer class is used to manage the state of the counter in the Redux store.
// It is a class that contains a method that takes the current state and an action and returns the new state.
using Fluxor;

namespace BlazorWithRedux.Store
{
    public static class CounterReducer
    {
        [ReducerMethod] // this attribute is used to mark the method as a reducer
        public static CounterState OnAddCounter(CounterState state, AddCounter action)
        {
            // we just want to return a new state with the count incremented by 1
            // we just need to pass what we want to change in the state, the rest of the state will be copied
            // duplicate the state and change the count property(deep copy, not a reference)
            return state with { Count = state.Count + 1 }; 
            // this behaviour is possible because the CounterState class is a record
            // if the CounterState class was a class, we would need to create a new instance of the class and copy the properties manually
        }

        // New reducer method
        [ReducerMethod]
        public static CounterState OnSubtractCounter(CounterState state, SubCounter action)
        {
            return state with { Count = state.Count - 1 };
        }
    }
}