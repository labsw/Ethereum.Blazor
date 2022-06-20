using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace Ether.BlazorProvider
{
    public static class BuilderConfigureExtension
    {
        public static void AddEtherProvider(this IServiceCollection services, Action<EtherProviderConfiguration>? options = null)
        {
            var etherProviderConfiguration = new EtherProviderConfiguration();

            if(options != null)
                options(etherProviderConfiguration);

            services.AddSingleton<IEtherProvider>(
                x => new EtherProvider(x.GetRequiredService<IJSRuntime>(), etherProviderConfiguration));
        }
    }
}
