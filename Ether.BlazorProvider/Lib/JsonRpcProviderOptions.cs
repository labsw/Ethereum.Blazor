using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ether.BlazorProvider
{
    public class JsonRpcProviderOptions
    {
        public string ProviderPath { get; set; } = string.Empty;
        public bool SupportsEip1193 { get; set; } = true;
    }


}
