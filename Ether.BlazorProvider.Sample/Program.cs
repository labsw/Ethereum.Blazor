using Ether.BlazorProvider.Sample;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Ether.BlazorProvider;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


// basic configuration (for MetaMask)
builder.Services.AddEtherProviderRegistry(config => config.AddMetaMaskProvider());


// advanced configuration
builder.Services.AddEtherProviderRegistry( config =>
    {
        // quick metamask configuration with custom name
        config.AddMetaMaskProvider("my-metamask");

        // example of disabling events
        //options.AddMetaMaskProvider("my-metamask").Configure(x => x.EnableEvents = false);

        // standard way to configure
        config.AddProvider("ronin")
            .Configure(x =>
            {
                x.ProviderPath = "ronin.provider";
                x.SupportsEip1193 = false;
                x.SupportsEip1102 = false;
            });
    }
);

await builder.Build().RunAsync();
