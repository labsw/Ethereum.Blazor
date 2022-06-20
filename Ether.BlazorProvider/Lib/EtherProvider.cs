using Ether.BlazorProvider.Internal;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ether.BlazorProvider
{
    public class EtherProvider : IEtherProvider
    {
        private IEtherInterop _etherInterop;

        public EtherProvider(IJSRuntime jsRuntime)
        {
            _etherInterop = new EtherInterop(jsRuntime);
        }

        public async ValueTask<IJsonRpcProvider> InitProvider(string name, JsonRpcProviderOptions options)
        {
            IJsonRpcProviderInterop providerInterop = await _etherInterop.InitProvider(name, options);
            var provider = new JsonRpcProvider(providerInterop, options);
            return provider;
        }


    }
}
