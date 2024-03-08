using Fluxor;
using System.Net.Http.Json;

namespace BlazorWithRedux.Store.Weather.Store
{
    public class WeatherEffects
    {
        private readonly HttpClient Http;

        /* This is the main difference between using effects or not, here we have
         * has a constructor into which we can inject dependencies, 
         * and which we can use in each EffectMethod.
         * So, we can inject HttpClient and use it to make HTTP requests in our effects.
         * Instead of using the HttpClient directly in the effect method, we inject it into the constructor.
         * Moreover we don't need to inject the HttpClient into the razor component
         * and pass it to the action creator, we can just inject it into the effect class.
         */
        public WeatherEffects(HttpClient http)
        {
            Http = http;
        }

        [EffectMethod]
        public async Task LoadForecasts(WeatherLoadForecastsAction action, IDispatcher dispatcher)
        {
            dispatcher.Dispatch(new WeatherSetLoadingAction(true));
            var forecasts = await Http.GetFromJsonAsync<WeatherForecast[]>("sample-data/weather.json");
            var randomForecasts = GetRandomForecasts(forecasts); // Get a randomly set of forecasts
            dispatcher.Dispatch(new WeatherSetForecastsAction(randomForecasts));
            dispatcher.Dispatch(new WeatherSetLoadingAction(false));
        }

        private WeatherForecast[] GetRandomForecasts(WeatherForecast[] forecasts)
        {
            var random = new Random();
            var randomForecasts = forecasts.OrderBy(x => random.Next()).ToArray();
            return randomForecasts;
        }
    }
}
