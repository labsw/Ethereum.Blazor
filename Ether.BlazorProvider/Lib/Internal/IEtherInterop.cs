using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ether.BlazorProvider.Internal
{
    internal interface IEtherInterop
    {
        ValueTask<IJsonRpcProviderInterop> InitProvider(string name, JsonRpcProviderOptionsDto options);
    }
}
