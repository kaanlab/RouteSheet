using FluentValidation;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using RouteSheet.Client;
using RouteSheet.Client.Validations;
using RouteSheet.Shared.ViewModels;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

await builder.Build().RunAsync();
