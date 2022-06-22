using Ether.BlazorProvider;
using Ether.NethereumProvider.Internal;
using Nethereum.Web3;

namespace Ether.NethereumProvider
{
    public class NetherumWeb3Provider : INethereumWeb3Provider, IDisposable
    {
        private readonly IJsonRpcProvider _jsonRpcProvider;
        private readonly JsonRpcProviderInterceptor _interceptor;
        private readonly IWeb3 _web3;

        private bool disposedValue;

        public NetherumWeb3Provider(IJsonRpcProvider jsonRpcProvider)
        {
            _jsonRpcProvider = jsonRpcProvider;
            _jsonRpcProvider.AccountChanged += AccountChanged;

            _interceptor = new JsonRpcProviderInterceptor(_jsonRpcProvider);
            _web3 = new Web3 { Client = { OverridingRequestInterceptor = _interceptor } };
        }

        public string Account => _interceptor.Account;

        public ValueTask<bool> IsProviderAvailable()
        {
            return _jsonRpcProvider.IsAvailable();
        }

        public async ValueTask Connect()
        {
            string account = await _jsonRpcProvider.Connect();
            AccountChanged(account);
        }


        public IWeb3 GetWeb3()
        {
            return _web3;
        }

        //--- 

        private void AccountChanged(string account)
        {
            _interceptor.UpdateAccount(account);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _jsonRpcProvider.AccountChanged -= AccountChanged;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

    }
}
