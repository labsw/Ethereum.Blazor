using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ether.BlazorProvider.Internal
{
    internal interface IJsonRpcProviderInterop
    {

        ValueTask<bool> IsAvailable();

        ValueTask<string> Request(string jsonRequest, long? timeoutInMs);
        ValueTask<RpcResponseMessageDto> Request(RpcRequestMessageDto request, long? timeoutInMs);
    }

    internal class JsonRpcProviderInterop : IJsonRpcProviderInterop, IAsyncDisposable
    {
        private readonly IJSObjectReference _jsProvider;

        public JsonRpcProviderInterop(IJSObjectReference jsProvider)
        {
            _jsProvider = jsProvider;
        }

        public ValueTask<bool> IsAvailable()
        {
            return _jsProvider.InvokeAsync<bool>("isAvailable");
        }

        public ValueTask<string> Request(string jsonRequest, long? timeoutInMs)
        {
            return _jsProvider.InvokeAsync<string>("request", jsonRequest, timeoutInMs);
        }

        public async ValueTask<RpcResponseMessageDto> Request(RpcRequestMessageDto request, long? timeoutInMs)
        {
            var requestJson = JsonConvert.SerializeObject(request);

            string responseJson = await Request(requestJson, timeoutInMs);

            var responseMessage = JsonConvert.DeserializeObject<RpcResponseMessageDto>(responseJson);

            if (responseMessage != null)
                return responseMessage;

            throw new EtherProviderException("Failed to deserialised response");
        }

        public async ValueTask DisposeAsync()
        {
            await _jsProvider.DisposeAsync();
        }

    }
}
