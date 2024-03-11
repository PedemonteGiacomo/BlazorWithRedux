using BlazorWithRedux.Store.Weather.State;

namespace BlazorWithRedux.Store.Weather.Actions
{
    public class WeatherSetForecastsAction
    {
        public WeatherForecast[] Forecasts { get; }

        public WeatherSetForecastsAction(WeatherForecast[] forecasts)
        {
            Forecasts = forecasts;
        }
    }
}
