// Purpose: Contains the CounterReducer class which is used to manage the state of the counter in the Redux store.
// The CounterReducer class is used to manage the state of the counter in the Redux store.
// It is a class that contains a method that takes the current state and an action and returns the new state.
using BlazorWithRedux.Store.Counter.Actions;
using BlazorWithRedux.Store.Counter.State;
using Fluxor;
using Fluxor.Undo;

namespace BlazorWithRedux.Store.Counter.Reducers
{
    /*
     * The CounterReducer class is used to manage the state of the counter in the Redux store.
     * In this case, the UndoableReducers<TState> class is parameterized with the type UndoableCounterState, 
     * which represents the state of the counter in the Redux store. 
     * This allows the CounterReducer class to work specifically with the UndoableCounterState type 
     * and utilize the undo and redo functionality provided by the base class for that specific state type.
     */
    public class CounterReducers : UndoableReducers<UndoableCounterState>
    {
        [ReducerMethod] // this attribute is used to mark the method as a reducer
        public static UndoableCounterState OnAddCounter(UndoableCounterState state, AddCounter action)
        //{
        //  This is how was before using the normal CounterState class without the possibility to make the user undo the state.
        //    // we just want to return a new state with the count incremented by 1
        //    // we just need to pass what we want to change in the state, the rest of the state will be copied
        //    // duplicate the state and change the count property(deep copy, not a reference)
        //    return state with { Count = state.Count + 1 }; 
        //    // this behaviour is possible because the CounterState class is a record
        //    // if the CounterState class was a class, we would need to create a new instance of the class and copy the properties manually
        //}
         => state.WithNewPresent(p => p with
         {
             Count = p.Count + 1,
         });

        // New reducer method
        [ReducerMethod]
        public static UndoableCounterState OnSubtractCounter(UndoableCounterState state, SubCounter action)
        => state.WithNewPresent(p => p with
        {
            Count = p.Count - 1,
        });
    }
}