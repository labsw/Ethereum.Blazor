using Ether.BlazorProvider;
using Newtonsoft.Json;

namespace Ether.NethereumProvider.Internal
{
    internal class JsonRpcProviderInterceptorService : IJsonRpcProviderInterceptorService
    {
        private readonly IJsonRpcProvider _jsonRpcProvider;

        public JsonRpcProviderInterceptorService(IJsonRpcProvider jsonRpcProvider)
        {
            _jsonRpcProvider = jsonRpcProvider;
        }

        public string Account { set; get; } = String.Empty;

        public async ValueTask<Nethereum.JsonRpc.Client.RpcMessages.RpcResponseMessage> Request(Nethereum.JsonRpc.Client.RpcMessages.RpcRequestMessage request)
        {
            string requestJson = JsonConvert.SerializeObject(request);
            string responseJson = await _jsonRpcProvider.Request(requestJson);

            var response = JsonConvert.DeserializeObject<Nethereum.JsonRpc.Client.RpcMessages.RpcResponseMessage>(responseJson);
            return response!;
        }
    }
}
