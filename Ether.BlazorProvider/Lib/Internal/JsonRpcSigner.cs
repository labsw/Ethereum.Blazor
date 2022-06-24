using Ether.BlazorProvider.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ether.BlazorProvider.Internal
{
    internal class JsonRpcSigner : ISigner
    {
        private IJsonRpcProvider _provider;

        public JsonRpcSigner(IJsonRpcProvider provider)
        {
            _provider = provider;
        }

        public async ValueTask<string> SignMessage(string message, string address)
        {
            string hexMessage = message.ToHexUTF8();
            var request = new RpcRequestMessage(1, "personal_sign", hexMessage, address);

            IRpcResponseMessage response = await _provider.Request(request);
            string signedMessage = response.ResultAs<string>();

            return signedMessage;
        }

        public async ValueTask<string> SignMessage(string message)
        {
            string address = await _provider.GetAccount();
            string signedMessage = await SignMessage(message, address);
            return signedMessage;
        }
    }
}
