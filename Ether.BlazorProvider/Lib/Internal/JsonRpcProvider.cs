using Ether.BlazorProvider.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ether.BlazorProvider.Internal
{
    internal class JsonRpcProvider : IJsonRpcProvider
    {
        private IJsonRpcProviderInterop _providerInterop;
        private readonly string _name;
        private readonly JsonRpcProviderOptions _options;
        private readonly JsonRpcSigner _signer;

        private string? _account;
        private long _chainId;

        private bool _configureEventsDone;

        public event Action<string>? AccountChanged;
        public event Action<long>? ChainIdChanged;

        public JsonRpcProvider(IJsonRpcProviderInterop providerInterop, string name, JsonRpcProviderOptions options)
        {
            _providerInterop = providerInterop;
            _name = name;
            _options = options;
            _signer = new JsonRpcSigner(this);
        }

        public JsonRpcProviderOptions Options => _options;

        public string Name => _name;

        public async ValueTask<string> Connect(TimeSpan? timeout = null)
        {
            if( _options.SupportsEip1102)
            {
                var result = await InternalRequest<string[]>("eth_requestAccounts", timeout);
                return result[0];
            }

            return await GetAccount();
        }

        public async ValueTask<string> GetAccount()
        {
            var result = await InternalRequest<string[]>("eth_accounts", null);
            if (result.Length == 0)
                throw new EtherProviderException("Not accounts found");

            return result[0];
        }

        public async ValueTask<long> GetChainId()
        {
            string chainIdHex = await InternalRequest<string>("eth_chainId",null);
            return HexConverter.HexToLong(chainIdHex);
        }

        public ValueTask<bool> IsAvailable()
        {
            return _providerInterop.IsAvailable();
        }

        public async ValueTask<IRpcResponseMessage> Request(RpcRequestMessage request, TimeSpan? timeout)
        {
            long? timeoutInMs = timeout.HasValue ? (long)(timeout.Value.TotalMilliseconds) : null;

            var responseDto = await _providerInterop.Request(new RpcRequestMessageDto(request),timeoutInMs);
            return new RpcResponseMessage(responseDto);
        }

        public ValueTask<string> Request(string request, TimeSpan? timeout)
        {
            long? timeoutInMs = timeout.HasValue ? (long)(timeout.Value.TotalMilliseconds) : null;

            return _providerInterop.Request(request,timeoutInMs);
        }

        public ISigner GetSigner()
        {
            return _signer;
        }

        //--

        internal async ValueTask ConfigureEvents()
        {
            if (!_configureEventsDone)
            {
                await _providerInterop.ConfigureEvents(OnAccountChanged, OnChainIdChanged);
                _configureEventsDone = true;
            }
        }

        private async ValueTask<T> InternalRequest<T>(string method, TimeSpan? timeout, params object[] p)
        {
            RpcRequestMessage request = new RpcRequestMessage(0, method,p);

            IRpcResponseMessage responseMessage = await Request(request, timeout);

            return responseMessage.ResultAs<T>();
        }

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
