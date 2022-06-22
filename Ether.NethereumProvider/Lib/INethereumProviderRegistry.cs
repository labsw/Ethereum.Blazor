using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ether.NethereumProvider
{
    public interface INethereumProviderRegistry
    {
        public ValueTask<INethereumWeb3Provider> GetSingleProvider();
        public ValueTask<INethereumWeb3Provider> GetProvider(string name);
    }
}
