using Nethereum.JsonRpc.Client.RpcMessages;

namespace Ether.NethereumProvider.Internal
{
    internal interface IMetaMaskInterceptorService
    {
        public string Account { get; set; }
        ValueTask<RpcResponseMessage> RpcRequest(RpcRequestMessage request);
    }
}
