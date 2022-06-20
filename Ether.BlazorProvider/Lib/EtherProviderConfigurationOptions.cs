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

        public JsonRpcProviderConfiguration AddProvider(string name)
        {
            if (_providerOptions.ContainsKey(name))
                throw new Exception($"Name {name} is already configured");

            var options = new JsonRpcProviderOptions();
            _providerOptions[name] = options;

            return new JsonRpcProviderConfiguration(options);
        }

        /// <summary>
        /// MetaMask default configuration
        /// </summary>
        public JsonRpcProviderConfiguration AddMetaMaskProvider(string name)
        {
            if (_providerOptions.ContainsKey(name))
                throw new Exception($"Name {name} is already configured");

            var options = JsonRpcProviderOptions.MetaMaskOptions;
            _providerOptions[name] = options;

            _providerOptions[name] = JsonRpcProviderOptions.MetaMaskOptions;

            return new JsonRpcProviderConfiguration(options);
        }

        internal JsonRpcProviderOptions? TryGetProviderOptions(string name)
        {
            if(_providerOptions.ContainsKey(name))
                return _providerOptions[name];

            return null;
        }
    }

    public class JsonRpcProviderConfiguration
    {
        private JsonRpcProviderOptions _options;

        public JsonRpcProviderConfiguration(JsonRpcProviderOptions options)
        {
            _options = options;
        }

        public void Configure(Action<JsonRpcProviderOptions>? options = null)
        {
            if (options != null)
                options(_options);
        }
    }

}
