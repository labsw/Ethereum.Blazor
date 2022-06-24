using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace Ether.BlazorProvider.Internal
{
    internal interface IJsonRpcProviderInterop
    {
        ValueTask<bool> IsAvailable();

        ValueTask ConfigureEvents(Action<string>? onAccountChanged, Action<long>? onChainChanged);

        ValueTask<string> Request(string jsonRequest, long? timeoutInMs);
        ValueTask<RpcResponseMessageDto> Request(RpcRequestMessageDto request, long? timeoutInMs);

        ValueTask RemoveEvents();
    }

    internal class JsonRpcProviderInterop : IJsonRpcProviderInterop, IAsyncDisposable
    {
        private readonly IJSObjectReference _jsProvider;

        private Action<string>? _onAccountChanged;
        private Action<long>? _onChainIdChanged;

        private DotNetObjectReference<JsonRpcProviderInterop>? _dotNetObjectReference = null;

        public JsonRpcProviderInterop(IJSObjectReference jsProvider)
        {
            _jsProvider = jsProvider;
        }

        public ValueTask<bool> IsAvailable()
        {
            return _jsProvider.InvokeAsync<bool>("isAvailable");
        }

        public async ValueTask ConfigureEvents(Action<string>? onAccountChanged, Action<long>? onChainChanged)
        {
            var dotNetObjectReference = DotNetObjectReference.Create(this);

            await _jsProvider.InvokeVoidAsync("configureEvents", dotNetObjectReference);

            _onAccountChanged = onAccountChanged;
            _onChainIdChanged = onChainChanged;
            _dotNetObjectReference = dotNetObjectReference;
        }

        public ValueTask<string> SignMessage(string message)
        {
            return _jsProvider.InvokeAsync<string>("signMessage", message);
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

        public async ValueTask RemoveEvents()
        {
            if( _dotNetObjectReference != null )
            {
                await _jsProvider.InvokeVoidAsync("removeEvents");
            }
       }

        public async ValueTask DisposeAsync()
        {
            await RemoveEvents();

            _dotNetObjectReference?.Dispose();

            _dotNetObjectReference = null;
            _onAccountChanged = null;
            _onChainIdChanged = null;

            await _jsProvider.DisposeAsync();
        }

        [JSInvokable]
        public Task AccountChanged(string account)
        {
            if (_onAccountChanged != null)
                _onAccountChanged(account);

            return Task.CompletedTask;
        }

        [JSInvokable]
        public Task ChainChanged(string chainIdStr)
        {
            long chainId = HexConverter.HexToLong(chainIdStr);

            if (_onChainIdChanged != null)
                _onChainIdChanged(chainId);

            return Task.CompletedTask;
        }

    }
}
