using Nethereum.JsonRpc.Client;
using Nethereum.JsonRpc.Client.RpcMessages;
using Nethereum.RPC.Eth.DTOs;
using Newtonsoft.Json;

namespace Ether.NethereumProvider.Internal
{
    internal class JsonRpcProviderInterceptor : RequestInterceptor
    {
        private readonly Ether.BlazorProvider.IJsonRpcProvider _jsonRpcProvider;
        private string _account = string.Empty;

        public JsonRpcProviderInterceptor(Ether.BlazorProvider.IJsonRpcProvider jsonRpcProvider)
        {
            _jsonRpcProvider = jsonRpcProvider;
        }

        public string Account => _account;

        public void UpdateAccount(string account)
        {
            _account = account;
        }

        public override async Task<object> InterceptSendRequestAsync<T>(
            Func<RpcRequest, string, Task<T>> interceptedSendRequestAsync,
            RpcRequest request,
#pragma warning disable  CS8625
            string route = null
#pragma warning restore  CS8625
            )
        {
            if (request.Method == "eth_sendTransaction")
            {
                string fromAddress = _account;

                var transaction = (TransactionInput)request.RawParameters[0];
                transaction.From = fromAddress;
                request.RawParameters[0] = transaction;

                var rpcRequest = new NethereumRpcRequestMessage(request.Id, request.Method, fromAddress, request.RawParameters);
                RpcResponseMessage response = await Request(rpcRequest);

                return ConvertResponse<T>(response);
            }
            else
            {
                var rpcRequest = new RpcRequestMessage(request.Id, request.Method, request.RawParameters);
                var response = await Request(rpcRequest);

                return ConvertResponse<T>(response);
            }
        }

        public override async Task<object> InterceptSendRequestAsync<T>(
            Func<string, string, object[], Task<T>> interceptedSendRequestAsync,
            string method,
#pragma warning disable  CS8625
            string route = null,
#pragma warning restore CS8625
            params object[] paramList)
        {

            if (method == "eth_sendTransaction")
            {
                string fromAddress = _account;

                var transaction = (TransactionInput)paramList[0];
                transaction.From = fromAddress;
                paramList[0] = transaction;

                var rpcRequest = new NethereumRpcRequestMessage(route, method, fromAddress, paramList);

                RpcResponseMessage response = await Request(rpcRequest);
                return ConvertResponse<T>(response);
            }
            else
            {
                var rpcRequest = new Nethereum.JsonRpc.Client.RpcMessages.RpcRequestMessage(route, method, paramList);

                RpcResponseMessage response = await Request(rpcRequest);
                return ConvertResponse<T>(response);
            }
        }

        //-- 

        private async ValueTask<Nethereum.JsonRpc.Client.RpcMessages.RpcResponseMessage> Request(Nethereum.JsonRpc.Client.RpcMessages.RpcRequestMessage request)
        {
            string requestJson = JsonConvert.SerializeObject(request);
            string responseJson = await _jsonRpcProvider.Request(requestJson);

            var response = JsonConvert.DeserializeObject<Nethereum.JsonRpc.Client.RpcMessages.RpcResponseMessage>(responseJson);
            return response!;
        }

        private void HandleRpcError(RpcResponseMessage response)
        {
            if (response.HasError)
                throw new RpcResponseException(new Nethereum.JsonRpc.Client.RpcError(response.Error.Code, response.Error.Message,
                    response.Error.Data));
        }

        private T ConvertResponse<T>(RpcResponseMessage response)
        {
            HandleRpcError(response);
            try
            {
                return response.GetResult<T>();
            }
            catch (FormatException formatException)
            {
                throw new RpcResponseFormatException("Invalid format found in RPC response", formatException);
            }
        }



    }
}
