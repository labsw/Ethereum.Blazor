using Nethereum.JsonRpc.Client;
using Nethereum.JsonRpc.Client.RpcMessages;
using Nethereum.RPC.Eth.DTOs;

namespace Ether.NethereumProvider.Internal
{
    internal class MetaMaskInterceptor : RequestInterceptor
    {
        private IMetaMaskInterceptorService _service;

        public MetaMaskInterceptor(IMetaMaskInterceptorService service)
        {
            _service = service;
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
                string fromAddress = _service.Account;

                var transaction = (TransactionInput)request.RawParameters[0];
                transaction.From = fromAddress;
                request.RawParameters[0] = transaction;

                var rpcRequest = new MetaMaskRpcRequestMessage(request.Id, request.Method, fromAddress, request.RawParameters);
                RpcResponseMessage response = await _service.RpcRequest(rpcRequest);

                return ConvertResponse<T>(response);
            }
            else
            {
                var rpcRequest = new RpcRequestMessage(request.Id, request.Method, request.RawParameters);
                var response = await _service.RpcRequest(rpcRequest);

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
                string fromAddress = _service.Account;

                var transaction = (TransactionInput)paramList[0];
                transaction.From = fromAddress;
                paramList[0] = transaction;

                var rpcRequest = new MetaMaskRpcRequestMessage(route, method, fromAddress, paramList);

                RpcResponseMessage response = await _service.RpcRequest(rpcRequest);
                return ConvertResponse<T>(response);
            }
            else
            {
                var rpcRequest = new RpcRequestMessage(route, method, paramList);

                RpcResponseMessage response = await _service.RpcRequest(rpcRequest);
                return ConvertResponse<T>(response);
            }
        }

        //-- 

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
