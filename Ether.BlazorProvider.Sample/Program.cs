using Ether.BlazorProvider.Sample;
using Ether.BlazorProvider;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// note: AddNethereumProvider() includes the MetaMask provider

// if you are only using the MetaMask provider then use AddMetaMaskProvider()
builder.Services.AddEtherProvider();

await builder.Build().RunAsync();
