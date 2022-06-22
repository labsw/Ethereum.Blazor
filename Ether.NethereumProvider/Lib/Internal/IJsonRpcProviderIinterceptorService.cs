using Nethereum.JsonRpc.Client.RpcMessages;

namespace Ether.NethereumProvider.Internal
{
    internal interface IJsonRpcProviderInterceptorService
    {
        public string Account { get; set; }
        ValueTask<RpcResponseMessage> Request(RpcRequestMessage request);
    }
}
