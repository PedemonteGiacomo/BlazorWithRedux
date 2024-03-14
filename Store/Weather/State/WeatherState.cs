using BlazorWithRedux.Store.Counter.State;
using Fluxor.Undo;
using System.Diagnostics.CodeAnalysis;

namespace BlazorWithRedux.Store.Weather.State
{
    public sealed record WeatherState
    {
        public bool Initialized { get; init; }
        public bool Loading { get; init; }
        public WeatherForecast[]? Forecasts { get; init; }
    }
    public sealed record UndoableWeatherState : Undoable<UndoableWeatherState, WeatherState> 
    {
        [SetsRequiredMembers]
        public UndoableWeatherState() => Present = new WeatherState()
        {
            Initialized = false,
            Loading = false,
            Forecasts = Array.Empty<WeatherForecast>()
        };
    }
}
