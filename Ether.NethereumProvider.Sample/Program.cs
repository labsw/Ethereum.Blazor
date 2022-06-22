using Ether.NethereumProvider.Sample;
using Ether.NethereumProvider;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Ether.BlazorProvider;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


// add and configure the Ether.BlazorProvider, as Nethereum Provider depends on it - see Ether.BlazorProvider for more details
builder.Services.AddEtherProviderRegistry(config =>
    {
        // quick metamask configuration
        config.AddMetaMaskProvider("my-metamask");
    }
);

// add Nethereum Provider Registry
builder.Services.AddNethereumProviderRegistry();

await builder.Build().RunAsync();
