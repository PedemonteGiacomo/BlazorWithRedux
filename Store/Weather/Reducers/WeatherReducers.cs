using BlazorWithRedux.Store.Weather.Actions;
using BlazorWithRedux.Store.Weather.State;
using Fluxor;
using Fluxor.Undo;

namespace BlazorWithRedux.Store.Weather.Reducers
{
    public class WeatherReducers : UndoableReducers<UndoableWeatherState>
    {
        [ReducerMethod]
        public static UndoableWeatherState OnSetForecasts(UndoableWeatherState state, WeatherSetForecastsAction action)
        => state.WithNewPresent(p => p with
        {
            Forecasts = action.Forecasts
        });

        [ReducerMethod]
        public static UndoableWeatherState OnSetLoading(UndoableWeatherState state, WeatherSetLoadingAction action)
        => state.WithNewPresent(p => p with
        {
            Loading = action.Loading
        });


        [ReducerMethod(typeof(WeatherSetInitializedAction))]
        public static UndoableWeatherState OnSetInitialized(UndoableWeatherState state)
        => state.WithNewPresent(p => p with
        {
            Initialized = true
        });
    }
}
