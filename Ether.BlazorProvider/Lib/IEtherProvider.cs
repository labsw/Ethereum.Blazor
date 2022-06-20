using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ether.BlazorProvider
{
    public interface IEtherProvider
    {
//        ValueTask<IJsonRpcProvider> InitProvider(string name, JsonRpcProviderOptions options);

        ValueTask<IJsonRpcProvider> GetProvider(string name);

    }
}
