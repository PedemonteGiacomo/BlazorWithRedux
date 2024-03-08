using Fluxor;

namespace BlazorWithRedux.Store.Weather.Store
{
    public static class WeatherReducers
    {
        [ReducerMethod]
        public static WeatherState OnSetForecasts(WeatherState state, WeatherSetForecastsAction action)
        {
            return state with
            {
                Forecasts = action.Forecasts
            };
        }

        [ReducerMethod]
        public static WeatherState OnSetLoading(WeatherState state, WeatherSetLoadingAction action)
        {
            return state with
            {
                Loading = action.Loading
            };
        }

        [ReducerMethod(typeof(WeatherSetInitializedAction))]
        public static WeatherState OnSetInitialized(WeatherState state)
        {
            return state with
            {
                Initialized = true
            };
        }
    }
}
