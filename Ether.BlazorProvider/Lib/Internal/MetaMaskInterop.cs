using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace Ether.BlazorProvider.Internal
{

    internal class MetaMaskInterop : IMetaMaskInterop, IAsyncDisposable
    {
        private const string _metaMaskServiceName = "MetaMaskInteropService";
        private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

        private Action<string>? _onAccountChanged;
        private Action<long>? _onChainIdChanged;

        private DotNetObjectReference<MetaMaskInterop>? _dotNetObjectReference = null;
        private bool _configureDone;

        public MetaMaskInterop(IJSRuntime jsRuntime)
        {
            _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/Ether.BlazorProvider/js/MetaMaskInterop.js").AsTask());

        }

        public ValueTask<bool> IsMetaMaskAvailable()
        {
            return InvokeInteropMethod<bool>("isMetaMaskAvailable");
        }

        public async ValueTask ConfigureOnChange(Action<string>? onAccountChanged, Action<long>? onChainChanged)
        {
            bool isMetamaskAvailable = await IsMetaMaskAvailable();

            if (!isMetamaskAvailable)
                throw new EtherProviderException("Metamask is not available");

            if (_configureDone)
                throw new EtherProviderException("Configure has already been called");

            var dotNetObjectReference = DotNetObjectReference.Create(this);

            await InvokeInteropMethod("configureOnChange", dotNetObjectReference);

            _onAccountChanged = onAccountChanged;
            _onChainIdChanged = onChainChanged;
            _dotNetObjectReference = dotNetObjectReference;
            _configureDone = true;
        }

        public async ValueTask<string> Connect()
        {
            string account = await InvokeInteropMethod<string>("connect");
            return account;
        }

        public async ValueTask<string> GetAccount()
        {
            var accounts = await InvokeInteropMethod<string[]>("getAccounts");
            if (accounts.Length > 0)
                return accounts[0];

            throw new EtherProviderException("No account available");
        }

        public async ValueTask<long> GetChainId()
        {
            string chainIdHex = await InvokeInteropMethod<string>("getChainId");
            return HexConverter.HexToLong(chainIdHex);
        }

        public ValueTask<string> SignMessage(string message)
        {
            string utf8Hex = message.ToHexUTF8();
            return InvokeInteropMethod<string>("sign",utf8Hex);
        }

        public async ValueTask<RpcResponseMessageDto> RpcRequest(RpcRequestMessageDto request)
        {
            var requestJson = JsonConvert.SerializeObject(request);
            string responseJson = await InvokeInteropMethod<string>("rpcRequest", requestJson);

            var responseMessage = JsonConvert.DeserializeObject<RpcResponseMessageDto>(responseJson);

            if( responseMessage != null)
                return responseMessage;

            throw new EtherProviderException("Failed to deserialised response");
        }

        public async ValueTask<string> RpcRequest(string request)
        {
            string response = await InvokeInteropMethod<string>("rpcRequest", request);
            return response;
        }


        public async ValueTask DisposeAsync()
        {
            if (_moduleTask.IsValueCreated)
            {
                var module = await _moduleTask.Value;
                await module.DisposeAsync();
            }

            _dotNetObjectReference?.Dispose();
            _onAccountChanged = null;
            _onChainIdChanged = null;
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

        //--

        private async ValueTask<T> InvokeInteropMethod<T>(string method, params object?[]? args)
        {
            try
            {
                var module = await _moduleTask.Value;
                var r = await module.InvokeAsync<T>($"{_metaMaskServiceName}.{method}", args);
                return r;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async ValueTask InvokeInteropMethod(string method, params object?[]? args)
        {
            try
            {
                var module = await _moduleTask.Value;
                await module.InvokeVoidAsync($"{_metaMaskServiceName}.{method}", args);
            }
            catch (Exception)
            {
                throw;
            }
        }




    }
}
