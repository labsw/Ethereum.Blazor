using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace Ether.BlazorProvider
{
    public static class BuilderConfigurationExt
    {
        /// <summary>
        /// An App builder extension use to populate the DI and configure providers
        /// </summary>
        public static void AddEtherProviderRegistry(this IServiceCollection services, Action<EtherProviderConfiguration>? options = null)
        {
            var etherProviderConfiguration = new EtherProviderConfiguration();

            options?.Invoke(etherProviderConfiguration);

            services.AddSingleton<IEtherProviderRegistry>(
                x => new EtherProviderRegistry(x.GetRequiredService<IJSRuntime>(), etherProviderConfiguration));
        }
    }
}
