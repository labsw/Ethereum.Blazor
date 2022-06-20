using Microsoft.Extensions.DependencyInjection;

namespace Ether.BlazorProvider
{
    public static class BuilderConfigureExtension
    {
        public static void AddMetaMaskProvider(this IServiceCollection services)
        {
            services.AddSingleton<IEtherProvider, EtherProvider>();
        }
    }
}
