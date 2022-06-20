using Nethereum.JsonRpc.Client.RpcMessages;
using Newtonsoft.Json;

namespace Ether.NethereumProvider.Internal
{
    internal class MetaMaskRpcRequestMessage : RpcRequestMessage
    {
        [JsonProperty("from")]
        public string From { get; private set; }

        public MetaMaskRpcRequestMessage(object id, string method, string from, params object[] parameterList)
            : base(id, method, parameterList)
        {
            From = from;
        }
    }
}
