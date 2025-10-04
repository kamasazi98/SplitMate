using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using SplitMate.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddMudServices();
builder.Services.AddHttpClient("MainApi", c => c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

builder.Services.AddWebClient();
await builder.Build().RunAsync();
