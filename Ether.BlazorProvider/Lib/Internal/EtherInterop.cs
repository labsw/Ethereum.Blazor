using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace Ether.BlazorProvider.Internal
{

    internal class EtherInterop : IEtherInterop, IAsyncDisposable
    {
        private const string _etherServiceName = "EtherInteropService";
        
        private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

        public EtherInterop(IJSRuntime jsRuntime)
        {
            _moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import",
                "./_content/Ether.BlazorProvider/js/EtherInterop.js"
                ).AsTask());
        }


        public async ValueTask<IJsonRpcProviderInterop> InitProvider(string name, JsonRpcProviderOptionsDto options)
        {
            var jsProvider = await InvokeInteropMethod<IJSObjectReference>("initProvider",name,options);
            var provider = new JsonRpcProviderInterop(jsProvider);

            return provider;
        }


        public async ValueTask DisposeAsync()
        {
            if (_moduleTask.IsValueCreated)
            {
                var module = await _moduleTask.Value;
                await module.DisposeAsync();
            }

        }

        //--

        private async ValueTask<T> InvokeInteropMethod<T>(string method, params object?[]? args)
        {
            try
            {
                var module = await _moduleTask.Value;
                var r = await module.InvokeAsync<T>($"{_etherServiceName}.{method}", args);
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
                await module.InvokeVoidAsync($"{_etherServiceName}.{method}", args);
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
