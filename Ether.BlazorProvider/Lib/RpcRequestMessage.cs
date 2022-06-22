
namespace Ether.BlazorProvider
{
    /// <summary>
    /// A RPC request message - see the Ethereum RPC spec for details
    /// </summary>
    public class RpcRequestMessage
    {
        public object Id { get; private set; }
        public string RpcVersion { get; private set; }
        public string Method { get; private set; }
        public object RawParameters { get; private set; }

        public RpcRequestMessage(object id, string method, params object[] parameterList)
        {
            Id = id;
            RpcVersion = "2.0";
            Method = method;
            RawParameters = parameterList;
        }

        public RpcRequestMessage(object id, string method, Dictionary<string, object> parameterMap)
        {
            Id = id;
            RpcVersion = "2.0";
            Method = method;
            RawParameters = parameterMap;
        }
    }


}
