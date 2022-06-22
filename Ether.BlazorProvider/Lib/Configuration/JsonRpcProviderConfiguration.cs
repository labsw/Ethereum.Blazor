using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ether.BlazorProvider
{
    public class JsonRpcProviderConfiguration
    {
        private EtherProviderConfiguration _configuration;
        private JsonRpcProviderOptions _options;

        public JsonRpcProviderConfiguration(JsonRpcProviderOptions options, EtherProviderConfiguration configuration)
        {
            _options = options;
            _configuration = configuration;
        }

        /// <summary>
        /// Configure the options of a provider
        /// </summary>
        public EtherProviderConfiguration Configure(Action<JsonRpcProviderOptions>? options = null)
        {
            if (options != null)
                options(_options);

            return _configuration;
        }
    }

}
