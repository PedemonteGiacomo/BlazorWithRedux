using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorWithRedux;
using Fluxor;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry.Exporter;
using System.Diagnostics;
using OpenTelemetry.Instrumentation;
using BlazorWithRedux.Store.Middleware.Logging;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var serviceName = "LoggingMiddleware";
var serviceVersion = "1.0.0";
var otlpEndpoint = "http://172.212.16.128";

// Add OpenTelemetry
builder.Services.AddOpenTelemetry()
    .WithTracing(b =>
    {
        b
            .SetSampler(new AlwaysOnSampler())
            .AddSource(serviceName)
            .ConfigureResource(resource =>
                  resource.AddService(
                    serviceName: serviceName,
                    serviceVersion: serviceVersion
                  )
             )
            .AddHttpClientInstrumentation()
            .AddOtlpExporter(opt =>
            {
                opt.Endpoint = new Uri(otlpEndpoint);
                opt.Protocol = OtlpExportProtocol.HttpProtobuf;
            });
            //.AddConsoleExporter();
    });

builder.Services.AddFluxor(options =>
    options
      .ScanAssemblies(typeof(Program).Assembly)
      .AddMiddleware<LoggingMiddleware>()
      .UseReduxDevTools()// optional, in production you can remove this line because is for debugging purposes
); 

await builder.Build().RunAsync();
