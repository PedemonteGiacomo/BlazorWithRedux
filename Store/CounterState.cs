using Fluxor;

namespace BlazorWithRedux.Store
{
    public record CounterState // the record keyword is a new feature in C# 9.0 that allows you to create immutable objects
    {
        public int Count { get; init; } // init will make the property immutable after the object is created
    }

    public class CounterFeature : Feature<CounterState> 
        // a feature is a class that represents a slice of the state
        // the state is the data that the application needs to keep track of
    {
        public override string GetName() => nameof(CounterState);

        protected override CounterState GetInitialState() => new CounterState { Count = 0 };
    }
}
