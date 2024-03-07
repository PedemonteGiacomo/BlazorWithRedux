// Purpose: Contains the CounterReducer class which is used to manage the state of the counter in the Redux store.
// The CounterReducer class is used to manage the state of the counter in the Redux store.
// It is a class that contains a method that takes the current state and an action and returns the new state.
using Fluxor;
using Fluxor.Undo;

namespace BlazorWithRedux.Store
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
        //{ how was before using the normal CounterState class without the possibility to make the user undo the state.
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

        /*
         * The OnUndoCounter method is a reducer method that takes the current state and an UndoAction<UndoableCounterState> action
         * The following reducers are used to handle the undo and redo actions for the counter state.
         * Those are taken from the abstarct class for all the possible type of undoable/reusable state.
         * 
         * The following methods are used to handle the undo and redo actions for the counter state type but they are not needed anymore
         * since we can directly use the abstract class that will be used with the UndoableCounterState class since we are using the UndoableReducers<TState> class.
         */

        //[ReducerMethod]
        //public static UndoableCounterState OnUndoCounter(UndoableCounterState state, UndoAction<UndoableCounterState> action)
        //    => state.WithUndoOne();

        //[ReducerMethod]
        //public static UndoableCounterState OnJumpCounter(UndoableCounterState state, JumpAction<UndoableCounterState> action)
        //    => state.WithJump(action.Amount);

        //[ReducerMethod]
        //public static UndoableCounterState OnUndoAllCounter(UndoableCounterState state, UndoAllAction<UndoableCounterState> action)
        //    => state.WithUndoAll();

        //[ReducerMethod]
        //public static UndoableCounterState OnRedoCounter(UndoableCounterState state, RedoAction<UndoableCounterState> action)
        //    => state.WithRedoOne();

        //[ReducerMethod]
        //public static UndoableCounterState OnRedoAllCounter(UndoableCounterState state, RedoAllAction<UndoableCounterState> action)
        //    => state.WithRedoAll();
    }
}