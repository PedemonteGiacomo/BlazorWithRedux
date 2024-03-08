﻿using Fluxor;
using Fluxor.Undo;

namespace BlazorWithRedux.Store.Weather.Store
{
    public sealed class UndoableWeatherFeature : Feature<UndoableWeatherState>
    {
        public override string GetName() => "Weather";

        protected override UndoableWeatherState GetInitialState()
        {
            return new()
            {
                Present = new WeatherState()
                {
                    Initialized = false,
                    Loading = false,
                    Forecasts = Array.Empty<WeatherForecast>()
                }
            };
        }
    }
}
