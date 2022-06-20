using Ether.BlazorProvider;
using Microsoft.Extensions.DependencyInjection;

namespace Ether.NethereumProvider
{
    public static class BuilderConfigureExt
    {
        public static void AddNethereumProvider(this IServiceCollection services)
        {
            services.AddSingleton<IMetaMaskProvider, MetaMaskProvider>();
            services.AddSingleton<INethereumMetaMaskProvider, NetherumMetaMaskProvider>();
        }
    }
}
