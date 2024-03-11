# BlazorWithRedux

## Problemi Past/Future handler of UndoableObjects:

### Problema 1: Gestione Corretta della Cronologia degli Undo

**Descrizione**: Quando si effettua un nuovo cambiamento di stato dopo aver eseguito delle operazioni di undo, i cambiamenti non vengono correttamente aggiunti alla cronologia dei passati, causando un reset del flusso degli undo.

**Cause**: Questo è il comportamento attuale della libreria, ma è necessario modificare l'implementazione.

**Soluzione Proposta:** 
- Potrebbe essere logico supporre che quando si torna indietro di un certo numero di azioni, si sia liberi di andare avanti, ma dal momento in cui si effettua un nuovo cambiamento, il futuro viene cancellato.
	- In questo modo si può capire che il flusso degli undo ha ripreso da un punto "seguibile".
		- Questo è importante perché, ad esempio, se il futuro è 14 e si è arrivati a 13, se si effettua un'azione che porta allo stesso stato, si avrebbe uno stato duplicato e potrebbe essere poco intuitivo tornare al futuro (poiché sarebbe lo stesso stato).
		- Questo potrebbe essere un problema quando si desidera tornare indietro a uno stato futuro come 50, andando indietro di 7/8 stati, ma avendo aggiornato lo stato, non si ha più accesso al futuro e si può solo tornare indietro e fare azioni di undo al posto di redo.

**Implementazione:** Il codice necessario per affrontare questo problema è già presente nel progetto. Bisogna fare un'attenta revisione e apportare le modifiche necessarie per garantire il corretto funzionamento della cronologia degli undo.


# Introduction

[Fluxor knowledge](https://www.notion.so/Fluxor-knowledge-aec7b9d86d7f40babaaec80280f04880?pvs=21)

# Implementing Undo/Redo actions

Referring to the content of the library [Fluxor.undo](https://github.com/Pjotrtje/Fluxor.Undo?tab=readme-ov-file):

[GitHub - Pjotrtje/Fluxor.Undo: Easy undo/redo for Fluxor](https://github.com/Pjotrtje/Fluxor.Undo?tab=readme-ov-file)

## Inside the `App.razor`:

```csharp
<Fluxor.Blazor.Web.StoreInitializer /> <!-- This is the default store initializer of fluxor -->
```

### Including in the `program.cs`:

```csharp
builder.Services.AddFluxor(options =>
{
	options
		.ScanAssemblies(typeof(Program).Assembly)
		.UseReduxDevTools(); // optional, in production you can remove this line because is for debugging purposes
});
```

## Usage

I implemented two different way to use this undo/redo states, one for the **`counter`** that rely on “pure functions” so without using Effects but only using Reducers (no need for async operations). On the other hand, the other implementation regards the **`weather`** component that use the effects to retrieve data async by simulating an api call that takes data from [sample-data/weather.json](https://github.com/PedemonteGiacomo/BlazorWithRedux/blob/master/wwwroot/sample-data/weather.json).

In both of the implementation I reach out the following usage:

### STATE

We start from a simple state that we want to make undoable

```csharp
public sealed record CounterState
{
    public int Count { get; init; }

    public int DoubleCount => Count * 2;

    public bool IsEven => Count % 2 == 0;

    public bool IsOdd => !IsEven;

    public bool IsNegative => Count < 0;

    public bool IsPositive => Count > 0;

    public bool IsZero => Count == 0;
}
```

So, we add the possibility to be undoable by adding this record:

```csharp
public sealed record UndoableCounterState : Undoable<UndoableCounterState, CounterState>;
```

This will take in account the undoable actions imported form the package Fluxor.Undo

### Uso delle keyword `record` e `sealed`:

<aside>
?? La keyword “record” ci permette di creare oggetti immutabili, il che ci serve eccome dato che non si vuole assolutamente modificare gli stati stessi ma si vuole avere la possibilità di creare stati analoghi con le modifiche gestite attraverso le azioni che il client sta eseguendo sull’applicazione. Inoltre si può direttamente modificare il campo/i campi di interesse e in automatico C# esegue la deep copy delle altre proprietà dello stato.

La keyword "sealed" in C# viene utilizzata per dichiarare una classe che non può essere ereditata da altre classi. Quando una classe viene dichiarata come "sealed", significa che non può essere estesa o derivata da altre classi. Questo vincolo è utile quando si desidera impedire l'estensione di una classe per motivi di sicurezza o per garantire che la classe rimanga immutabile.

</aside>

### FEATURE

```csharp
public sealed class UndoableCounterFeature : Feature<UndoableCounterState>
{
    public override string GetName() => nameof(CounterState);

    protected override UndoableCounterState GetInitialState() 
	    => new() { Present = new CounterState { Count = 0 } };
}
```

This is interesting because the *Undoable functionality* is based on the concepts of:

- **Present** ? the present is were we are in the moment, what the client is currently visualizing
    - so, in the future we need to change the way we return the record of the `CounterState`, because we need to return the `UndoableCounterState`
- **Past** ? previous actions made by the user are listed in “past actions”
- **Future** ? when we perform some undo actions, the actions that before were in the past, when coming back, goes to the future.
    - when the user go back to a certain state and then perform some other actions that change the state, the future is cancelled and the state before the action is added to the past list.

### ACTIONS

```csharp
public record AddCounter { }
public record SubCounter { }
```

Those are the actions that will change the state of our Counter that we save the state every changes of the counter value.

Inside the page were we handle client gesture or client interaction with the application we inject the Fluxor Dispatcher that triggers the actions that we need to perform to the reducer that is listening to them, moreover we need to inject the UndoableState to display correctly the property of this state that we want:

```csharp
@inject IState<UndoableCounterState> UndoableCounterState
@inject IDispatcher Dispatcher
```

So, we can handle the actions by calling them throught the dispatcher mentioned above:

```csharp
<button class="btn btn-primary" @onclick="IncrementCount">Add +1</button>

...

@code {
	...
	private void IncrementCount()
	{
	    Dispatcher.Dispatch(new AddCounter()); // Dispatch the action to the store
	}
	...
}
```

Since we have declared the state as undoable, we can also using and calling the undoable actions as desired without the needs to redifine those actions, the reducer of those undoable actions are already implemented in the Fluxor.Undo package with a generic type, so we can simply pass as type of those actions the `UndoableCounterState` for example:

```csharp
<button class="btn btn-secondary" @onclick=@(() => Dispatcher.Dispatch(new UndoAllAction<UndoableCounterState>())) disabled="@UndoableCounterState.Value.HasNoPast">&lt;&lt;</button>
<button class="btn btn-secondary" @onclick=@(() => Dispatcher.Dispatch(new UndoAction<UndoableCounterState>())) disabled="@UndoableCounterState.Value.HasNoPast">&lt;</button>
<button class="btn btn-secondary" @onclick=@(() => Dispatcher.Dispatch(new RedoAction<UndoableCounterState>())) disabled="@UndoableCounterState.Value.HasNoFuture">&gt;</button>
<button class="btn btn-secondary" @onclick=@(() => Dispatcher.Dispatch(new RedoAllAction<UndoableCounterState>())) disabled="@UndoableCounterState.Value.HasNoFuture">&gt;&gt;</button>
```

You can also jump to certain previous actions or future once depending on what you need to do by using the `JumpAction` provided by the library:

```csharp
<button class="btn btn-primary" @onclick="JumpToPreviousState" disabled="@(errorIfJumpingToInvalidStatePAST)">Jump to Previous State</button>
<button class="btn btn-primary" @onclick="JumpToFutureState" disabled="@(errorIfJumpingToInvalidStateFUTURE)">Jump to Future State</button>

private void JumpToPreviousState()
{
    if (jumpAmount <= UndoableCounterState.Value.Past.Count)
    {
        Dispatcher.Dispatch(new JumpAction<UndoableCounterState>(-jumpAmount));
    }
}

private void JumpToFutureState()
{
    if (jumpAmount <= UndoableCounterState.Value.Future.Count)
    {
        Dispatcher.Dispatch(new JumpAction<UndoableCounterState>(jumpAmount));
    }
}
```

### REDUCERS

The reducers are methods used to “reduce” so perform the actions that they are prepared to handle, so they’re triggered when the action is called as we have seen above by, for example, the client.

```csharp
public class CounterReducers : UndoableReducers<UndoableCounterState>
{
    [ReducerMethod] // this attribute is used to mark the method as a reducer
    public static UndoableCounterState OnAddCounter(UndoableCounterState state, AddCounter action)
    //{
    //  This is how was before using the normal CounterState class without the possibility to make the user undo the state.
    //    // we just want to return a new state with the count incremented by 1
    //    // we just need to pass what we want to change in the state, the rest of the state will be copied
    //    // duplicate the state and change the count property(deep copy, not a reference)
    //    return state with { Count = state.Count + 1 }; 
    //    // this behaviour is possible because the CounterState class is a record
    //    // if the CounterState class was a class, we would need to create a new instance of the class and copy the properties manually
    //}
     => state.WithNewPresent(p => p with
     {
         Count = p.Count + 1,
     });

    // New reducer method
    [ReducerMethod]
    public static UndoableCounterState OnSubtractCounter(UndoableCounterState state, SubCounter action)
    => state.WithNewPresent(p => p with
    {
        Count = p.Count - 1,
    });
}
```

Respecting on the simple usage of the state (reported in green in the comments) that return the state with the property that we want to change, we return the ***present state*** with the property value changed as wanted.

### EFFECTS

In the implementation of the [Weather](https://github.com/PedemonteGiacomo/BlazorWithRedux/tree/master/Store/Weather) component using Redux Fluxor Undoable states, the usage of the effects was needed because the main purpose of this component is to retrieve data from a “simulated” API, so retrieving data from a JSON file and display 10 results of this retreived data that changes randomly every time we fetch or load those “*WeatherForecasts*”

```csharp
<button class="btn btn-outline-info" @onclick="LoadForecasts">Refresh Forecasts</button>
```

So, I declared the methods that will call the correct dispatcher for all those actions:

```csharp
protected override void OnInitialized()
{
    if (UndoableWeatherState.Value.Present.Initialized == false)
    {
        LoadForecasts();
        Dispatcher.Dispatch(new WeatherSetInitializedAction());
    }
    base.OnInitialized();
}

private void LoadForecasts()
{
    Dispatcher.Dispatch(new WeatherLoadForecastsAction());
}
```

The `WeatherSetInitializedAction()` is an action that is taken into account by the simple weather reducer because don’t need to wait any operations so we don’t need to introduce the `async/await` architecture as we have seen above in the `CounterState` and make manage this state properties by the “*pure functions*” **of the `WeatherReducers`.

On the other hand, the `WeatherLoadForecastsAction` has to be handled by an `Effect`. This because we need to handle the request as an `HttpClient`, if we don’t use the effect we need to inject the http client directly in the component in the client side and make the client perform the request in this way:

```csharp
private async Task LoadForecasts()
{
    Dispatcher.Dispatch(new WeatherSetLoadingAction(true));
    var forecasts = await Http.GetFromJsonAsync<WeatherForecast[]>("sample-data/weather.json");
    Dispatcher.Dispatch(new WeatherSetForecastsAction(forecasts));
    Dispatcher.Dispatch(new WeatherSetLoadingAction(false));
}
```

The main differences of those two approaches is the usage of the separation of concerns that Fluxor provides with its usage. The effect is responsible for handling side effects, such as making HTTP requests, while the component focuses on rendering the UI and handling user interactions. This separation allows for better code organization and maintainability.

Another advantage is that by injecting the HttpClient into the effect's constructor, you can easily mock or replace the HttpClient for testing purposes. This makes it easier to write unit tests for your effects without the need for actual network requests.

```csharp
public class WeatherEffects
{
    private readonly HttpClient Http;

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
```

For example, let's say you want to test the LoadForecasts method in the WeatherEffects class. Without dependency injection, you would need to create an instance of HttpClient within the method and make an actual network request to retrieve the forecasts. This can make your tests slower and less reliable, as they would depend on external factors such as network connectivity and the availability of the server.

As we can see we rely always on reducers but the `EffectMethod` makes us obtain the handle of the async functions and make the further calls to the reducer consistent, so respecting to the previous code we should consider more the following part:

```csharp
/* Retrieve the forecasts data */
var forecasts = await Http.GetFromJsonAsync<WeatherForecast[]>("sample-data/weather.json");
/* Shuffle the forecasts to simulate random data */
var randomForecasts = GetRandomForecasts(forecasts); // Get a randomly set of forecasts
/* Dispatch the reducer with the obtained data */
dispatcher.Dispatch(new WeatherSetForecastsAction(randomForecasts));
```

## Final Result with Redux DevTools extensions

Use the Redux DevTools to inspect the state of the application and the actions that are dispatched. This is useful for debugging and understanding how the state changes over time.

# Conclusion

The usage of the Fluxor.Undo package is very useful to handle the state of the application and make the user able to undo and redo the actions that he made in the application. 

The usage of the Redux DevTools is also very useful to inspect the state of the application and the actions that are dispatched. This is useful for debugging and understanding how the state changes over time.