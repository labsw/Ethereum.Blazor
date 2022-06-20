using Ether.BlazorProvider;
using Ether.NethereumProvider.Internal;
using Nethereum.Web3;

namespace Ether.NethereumProvider
{
    public class NetherumMetaMaskProvider : INethereumMetaMaskProvider, IDisposable
    {
        private readonly IMetaMaskProvider _metaMaskProvider;
        private readonly MetaMaskInterceptor _metaMaskInterceptor;
        private readonly IMetaMaskInterceptorService _metaMaskInterceptorService;

        private bool disposedValue;

        public NetherumMetaMaskProvider(IMetaMaskProvider metaMaskProvider)
        {
            _metaMaskProvider = metaMaskProvider;
            _metaMaskProvider.AccountChanged += AccountChanged;

            _metaMaskInterceptorService = new MetaMaskInterceptorService(_metaMaskProvider);
            _metaMaskInterceptor = new MetaMaskInterceptor(_metaMaskInterceptorService);
        }

        public string Account => _metaMaskInterceptorService.Account;

        public ValueTask<bool> IsMetaMaskAvailable()
        {
            return _metaMaskProvider.IsMetaMaskAvailable();
        }

        public async ValueTask Connect()
        {
            string account = await _metaMaskProvider.Connect();
            _metaMaskInterceptorService.Account = account;
            await _metaMaskProvider.ListenForChanges();
        }


        public ValueTask<IWeb3> CreateWeb3()
        {
            var web3 = new Web3 { Client = { OverridingRequestInterceptor = _metaMaskInterceptor } };
            return ValueTask.FromResult((IWeb3)web3);
        }

        //--- 

        private void AccountChanged(string account)
        {
            _metaMaskInterceptorService.Account = account;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _metaMaskProvider.AccountChanged -= AccountChanged;
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

        public ValueTask<bool> IsMetaMaskAvailability()
        {
            throw new NotImplementedException();
        }
    }
}
