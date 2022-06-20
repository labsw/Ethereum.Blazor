using Ether.BlazorProvider;
using Newtonsoft.Json;

namespace Ether.NethereumProvider.Internal
{
    internal class MetaMaskInterceptorService : IMetaMaskInterceptorService
    {
        private readonly IMetaMaskProvider _metaMaskProvider;

        public MetaMaskInterceptorService(IMetaMaskProvider metaMaskProvider)
        {
            _metaMaskProvider = metaMaskProvider;
        }

        public string Account { set; get; } = String.Empty;

        public async ValueTask<Nethereum.JsonRpc.Client.RpcMessages.RpcResponseMessage> RpcRequest(Nethereum.JsonRpc.Client.RpcMessages.RpcRequestMessage request)
        {
            string requestJson = JsonConvert.SerializeObject(request);
            string responseJson = await _metaMaskProvider.RpcRequest(requestJson);

            var response = JsonConvert.DeserializeObject<Nethereum.JsonRpc.Client.RpcMessages.RpcResponseMessage>(responseJson);
            return response!;
        }
    }
}
