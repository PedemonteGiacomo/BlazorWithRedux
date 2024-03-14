using Fluxor.Undo;

namespace BlazorWithRedux.Store.Weather.State
{
    public sealed record WeatherState
    {
        public bool Initialized { get; init; }
        public bool Loading { get; init; }
        public WeatherForecast[]? Forecasts { get; init; }
    }
    public sealed record UndoableWeatherState : Undoable<UndoableWeatherState, WeatherState>;
}
