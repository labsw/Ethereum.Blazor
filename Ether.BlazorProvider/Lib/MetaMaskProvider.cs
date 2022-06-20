using Microsoft.JSInterop;
using Ether.BlazorProvider.Internal;

namespace Ether.BlazorProvider
{
    public class MetaMaskProvider : IMetaMaskProvider
    {
        private readonly IMetaMaskInterop _metaMaskInterop;
        private string? _account;
        private long _chainId;
        private bool _listenConfigured;

        public MetaMaskProvider(IJSRuntime jsRuntime)
        {
            _metaMaskInterop = new MetaMaskInterop(jsRuntime);
        }

        public event Action<string>? AccountChanged;
        public event Action<long>? ChainIdChanged;

        public ValueTask<bool> IsMetaMaskAvailable()
        {
            return _metaMaskInterop.IsMetaMaskAvailable();
        }

        public ValueTask<string> Connect()
        {
            return _metaMaskInterop.Connect();
        }

        public ValueTask<string> GetAccount()
        {
            return _metaMaskInterop.GetAccount();
        }

        public ValueTask<long> GetChainId()
        {
            return _metaMaskInterop.GetChainId();
        }

        public async ValueTask ListenForChanges()
        {
            if(!_listenConfigured)
            {
                await _metaMaskInterop.ConfigureOnChange(OnAccountChanged, OnChainIdChanged);
                _listenConfigured = true;
            }
        }

        public ValueTask<string> SignMessage(string message)
        {
            return _metaMaskInterop.SignMessage(message);
        }

        public async ValueTask<IRpcResponseMessage> RpcRequest(RpcRequestMessage rpcRequestMessage)
        {
            var responseDto = await _metaMaskInterop.RpcRequest( new RpcRequestMessageDto(rpcRequestMessage));
            return new RpcResponseMessage(responseDto);
        }

        public ValueTask<string> RpcRequest(string request)
        {
            return _metaMaskInterop.RpcRequest(request);
        }

        //--- 

        private void UpdateAccount(string account)
        {
            if (_account != account)
            {
                _account = account;
                if (AccountChanged != null)
                    AccountChanged.Invoke(account);
            }
        }

        private void UpdateChainId(long chainId)
        {
            if (_chainId != chainId)
            {
                _chainId = chainId;
                if (ChainIdChanged != null)
                    ChainIdChanged.Invoke(chainId);
            }
        }

        private void OnAccountChanged(string account)
        {
            UpdateAccount(account);
        }

        private void OnChainIdChanged(long chainId)
        {
            UpdateChainId(chainId);
        }

    }
}
