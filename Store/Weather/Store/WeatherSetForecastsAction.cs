using BlazorWithRedux.Store.Weather.Pages;

namespace BlazorWithRedux.Store.Weather.Store
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
