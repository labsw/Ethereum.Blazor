using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ether.BlazorProvider.Internal
{
    internal class JsonRpcProviderOptionsDto
    {
        public string ProviderPath { get; private set; }
        public bool SupportsEip1193 { get; private set; }

        public JsonRpcProviderOptionsDto(string providerPath, bool supportsEip1193)
        {
            ProviderPath = providerPath;
            SupportsEip1193 = supportsEip1193;
        }
    }


}
