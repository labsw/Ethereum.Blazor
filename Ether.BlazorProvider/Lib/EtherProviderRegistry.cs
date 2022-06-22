using Ether.BlazorProvider.Internal;
using Microsoft.JSInterop;

namespace Ether.BlazorProvider
{
    public class EtherProviderRegistry : IEtherProviderRegistry
    {
        private readonly IEtherInterop _etherInterop;
        private readonly EtherProviderConfiguration _etherProviderConfiguration;

        private Dictionary<string, IJsonRpcProvider> _providers = new Dictionary<string, IJsonRpcProvider>();
        private SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

        public EtherProviderRegistry(IJSRuntime jsRuntime, EtherProviderConfiguration etherProviderConfiguration)
        {
            _etherInterop = new EtherInterop(jsRuntime);
            _etherProviderConfiguration = etherProviderConfiguration;
        }

        public bool HasSingleProvider => _etherProviderConfiguration.HasSingleProvider;

        public async ValueTask<IJsonRpcProvider> GetProvider(string name)
        {
            await _lock.WaitAsync();
            try
            {
                if (_providers.TryGetValue(name, out IJsonRpcProvider? provider))
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
            finally
            {
                _lock.Release();
            }
        }

        public async ValueTask<IJsonRpcProvider> GetSingleProvider()
        {
            if (!HasSingleProvider)
                throw new EtherProviderException($"Exactly one provider must be configured");

            await _lock.WaitAsync();
            try
            {
                if (_providers.Count == 1)
                    return _providers.First().Value;

                KeyValuePair<string, JsonRpcProviderOptions>? keyValuePair = _etherProviderConfiguration.TryGetSingleProviderOptions();

                // this is a sanity check - as the HasSingleProvider check above should guard against this ever occuring
                if (keyValuePair == null)
                    throw new EtherProviderException($"Exactly one provider must be configred");

                string name = keyValuePair.Value.Key;

                IJsonRpcProvider? newProvider = await InitProvider(name, keyValuePair.Value.Value);
                _providers.Add(name, newProvider);

                return newProvider;
            }
            finally
            {
                _lock.Release();
            }
        }

        //--

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
