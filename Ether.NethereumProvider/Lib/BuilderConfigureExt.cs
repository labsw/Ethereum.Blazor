using Ether.BlazorProvider;
using Microsoft.Extensions.DependencyInjection;

namespace Ether.NethereumProvider
{
    public static class BuilderConfigureExt
    {
        public static void AddNethereumProviderRegistry(this IServiceCollection services)
        {
            services.AddSingleton<INethereumProviderRegistry, NethereumProviderRegistry>();
        }

    }
}
