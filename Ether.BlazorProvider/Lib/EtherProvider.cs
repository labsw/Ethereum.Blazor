using Ether.BlazorProvider.Internal;
using Microsoft.JSInterop;

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
            var optionsDto = BuildOptopnsDto(options);

            IJsonRpcProviderInterop providerInterop = await _etherInterop.InitProvider(name, optionsDto);
            var provider = new JsonRpcProvider(providerInterop, name, options);
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
