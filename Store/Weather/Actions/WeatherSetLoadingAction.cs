namespace BlazorWithRedux.Store.Weather.Actions
{
    public class WeatherSetLoadingAction
    {
        public bool Loading { get; }

        public WeatherSetLoadingAction(bool loading)
        {
            Loading = loading;
        }
    }
}
