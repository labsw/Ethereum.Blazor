using Ether.BlazorProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ether.NethereumProvider
{
    public class NethereumProviderRegistry : INethereumProviderRegistry
    {
        private IEtherProviderRegistry _etherProvider;

        private SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

        private readonly Dictionary<string, INethereumWeb3Provider> _web3Providers = new Dictionary<string, INethereumWeb3Provider>();

        public NethereumProviderRegistry(IEtherProviderRegistry etherProvider)
        {
            _etherProvider = etherProvider;
        }

        public async ValueTask<INethereumWeb3Provider> GetSingleProvider()
        {
            if (!_etherProvider.HasSingleProvider)
                throw new NethereumProviderException("Exactly one provider must be configured");

            await _lock.WaitAsync();
            try
            {
                if (_web3Providers.Count == 1)
                    return _web3Providers.First().Value;

                IJsonRpcProvider jsonRpcProvider = await _etherProvider.GetSingleProvider();
                var newWeb3Provider = new NetherumWeb3Provider(jsonRpcProvider);
                _web3Providers.Add(jsonRpcProvider.Name, newWeb3Provider);

                return newWeb3Provider;
            }
            finally
            {
                _lock.Release();
            }
        }

        public async ValueTask<INethereumWeb3Provider> GetProvider(string name)
        {
            await _lock.WaitAsync();
            try
            {
                // we have cached the provider
                if (_web3Providers.TryGetValue(name, out INethereumWeb3Provider? web3Provider))
                {
                    return web3Provider;
                }

                // lookup by name, will throw if name does not exist
                IJsonRpcProvider jsonRpcProvider = await _etherProvider.GetProvider(name);
                var newWeb3Provider = new NetherumWeb3Provider(jsonRpcProvider);
                _web3Providers.Add(name, newWeb3Provider);

                return newWeb3Provider;
            }
            finally
            {
                _lock.Release();
            }
        }
    }
}
