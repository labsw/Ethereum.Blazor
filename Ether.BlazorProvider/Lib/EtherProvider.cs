using Ether.BlazorProvider.Internal;
using Microsoft.JSInterop;

namespace Ether.BlazorProvider
{
    public class EtherProvider : IEtherProvider
    {
        private readonly IEtherInterop _etherInterop;
        private readonly EtherProviderConfiguration _etherProviderConfiguration;

        private Dictionary<string, IJsonRpcProvider> _providers = new Dictionary<string, IJsonRpcProvider>();

        public EtherProvider(IJSRuntime jsRuntime, EtherProviderConfiguration etherProviderConfiguration)
        {
            _etherInterop = new EtherInterop(jsRuntime);
            _etherProviderConfiguration = etherProviderConfiguration;
        }

        public async ValueTask<IJsonRpcProvider> GetProvider(string name)
        {
            // todo: locks?

            if( _providers.TryGetValue(name, out IJsonRpcProvider? provider) )
            {
                return provider;

            }

            JsonRpcProviderOptions? jsonRpcProviderOptions = _etherProviderConfiguration.TryGetProviderOptions(name);
            if (jsonRpcProviderOptions == null)
                throw new EtherProviderException($"Provider {name} has not been configured");

            IJsonRpcProvider? newProvider = await InitProvider(name, jsonRpcProviderOptions);
            _providers.Add(name, newProvider);

            return newProvider;
        }

        private async ValueTask<IJsonRpcProvider> InitProvider(string name, JsonRpcProviderOptions options)
        {
            var optionsDto = BuildOptopnsDto(options);

            IJsonRpcProviderInterop providerInterop = await _etherInterop.InitProvider(name, optionsDto);
            var provider = new JsonRpcProvider(providerInterop, name, options);

            if( options.EnableEvents)
            {
                await provider.ConfigureEvents();
            }

            return provider;
        }

        //-- 

        private JsonRpcProviderOptionsDto BuildOptopnsDto(JsonRpcProviderOptions options)
        {
            var dto = new JsonRpcProviderOptionsDto(
                providerPath:BuildProviderPath(options.ProviderPath),
                supportsEip1193: options.SupportsEip1193);

            return dto;
        }

        private string BuildProviderPath(string p)
        {
            if( p.StartsWith("window."))
            {
                return p.Substring("window.".Length);
            }

            return p;
        }


    }
}
