using Fluxor;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace BlazorWithRedux.Store.Middleware.Logging
{
    public class LoggingMiddleware : IMiddleware
    {
        private IStore Store;
        private IDispatcher Dispatcher;

        private static readonly ActivitySource MyActivitySource = new ActivitySource("LoggingMiddleware");

        public void AfterDispatch(object action)
        {
            //tracer.StartActiveSpan($"Action {action.GetType().Name} dispatched", SpanKind.Client); // , SpanKind.Client
            using (var activity = MyActivitySource.StartActivity("AfterDispatch"))
            {
                Console.WriteLine($"Action {action.GetType().Name} dispatched");
            }
        }

        public void AfterInitializeAllMiddlewares()
        {
            using (var activity = MyActivitySource.StartActivity("AfterInitializeAllMiddlewares"))
            {
                Console.WriteLine(nameof(AfterInitializeAllMiddlewares));
            }
            //tracer.StartActiveSpan(nameof(AfterInitializeAllMiddlewares), SpanKind.Client); // , SpanKind.Client
        }

        public void BeforeDispatch(object action)
        {
            using (var activity = MyActivitySource.StartActivity("BeforeDispatch"))
            {
                Console.WriteLine($"Action {action.GetType().Name} is about to be dispatched");
            }
            //tracer.StartActiveSpan($"Action {action.GetType().Name} is about to be dispatched", SpanKind.Client);
        }

        public IDisposable BeginInternalMiddlewareChange()
        {
            throw new NotImplementedException();
        }

        public Task InitializeAsync(IDispatcher dispatcher, IStore store)
        {
            Store = store;
            Dispatcher = dispatcher;
            using (var activity = MyActivitySource.StartActivity("InitializeAsync"))
            {
                Console.WriteLine(nameof(InitializeAsync));
            }
            //tracer.StartActiveSpan(nameof(InitializeAsync), SpanKind.Client);

            return Task.CompletedTask;
        }

        public bool MayDispatchAction(object action)
        {
            //tracer.StartActiveSpan(nameof(MayDispatchAction) + action.GetType().Name, SpanKind.Client);
            using (var activity = MyActivitySource.StartActivity("MayDispatchAction"))
            {
                Console.WriteLine(nameof(MayDispatchAction) + action.GetType().Name);
            }
            return true;
        }
    }
}
