using BlazorWithRedux.Store.Weather;

namespace BlazorWithRedux.Store
{
    public record WeatherState
    {
        public bool Initialized { get; init; }
        public bool Loading { get; init; }
        public WeatherForecast[] Forecasts { get; init; }
    }
}
