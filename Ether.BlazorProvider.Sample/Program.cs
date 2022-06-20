using Ether.BlazorProvider.Sample;
using Ether.BlazorProvider;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddEtherProvider( options =>
    {

        // quick metamask configuration - you still need to provide a name
        options.AddMetaMaskProvider("my-metamask");

        // example of disabling events
        //options.AddMetaMaskProvider("my-metamask").Configure(x => x.EnableEvents = false);

        // standard way to configure
        options.AddProvider("ronin")
            .Configure(x =>
            {
                x.ProviderPath = "ronin.provider";
                x.SupportsEip1193 = false;
            });
    }
);

await builder.Build().RunAsync();
