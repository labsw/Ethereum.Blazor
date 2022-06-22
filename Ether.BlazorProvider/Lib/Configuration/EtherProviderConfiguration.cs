using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ether.BlazorProvider
{
    public class EtherProviderConfiguration
    {
        private Dictionary<string, JsonRpcProviderOptions> _providerOptions = new Dictionary<string, JsonRpcProviderOptions>();

        /// <summary>
        /// Configure a new provider. The name is arbitrary but must be unique amount all the providers.
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="EtherProviderException">This will be thrown if the name is not unique</exception>
        public JsonRpcProviderConfiguration AddProvider(string name)
        {
            if (_providerOptions.ContainsKey(name))
                throw new EtherProviderException($"Name {name} is already configured");

            var options = new JsonRpcProviderOptions();
            _providerOptions[name] = options;

            return new JsonRpcProviderConfiguration(options,this);
        }

        /// <summary>
        /// Configure a default MetaMask provider
        /// </summary>
        public JsonRpcProviderConfiguration AddMetaMaskProvider(string name = "metamask")
        {
            if (_providerOptions.ContainsKey(name))
                throw new Exception($"Name {name} is already configured");

            var options = JsonRpcProviderOptions.MetaMaskOptions;
            _providerOptions[name] = options;

            _providerOptions[name] = JsonRpcProviderOptions.MetaMaskOptions;

            return new JsonRpcProviderConfiguration(options,this);
        }

        //--

        internal bool HasSingleProvider => _providerOptions.Count == 1;

        internal JsonRpcProviderOptions? TryGetProviderOptions(string name)
        {
            if(_providerOptions.ContainsKey(name))
                return _providerOptions[name];

            return null;
        }

        internal KeyValuePair<string,JsonRpcProviderOptions>? TryGetSingleProviderOptions()
        {
            if (_providerOptions.Count == 1)
            {
                return _providerOptions.First();
            }

            return null;
        }

    }


}
