using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ether.BlazorProvider
{
    public class JsonRpcProviderOptions
    {
        /// <summary>
        /// Javascript provider object path
        /// </summary>
        public string ProviderPath { get; set; } = string.Empty;

        /// <summary>
        /// A flag indicating whether the provider supports the Eip1193 API
        /// </summary>
        public bool SupportsEip1193 { get; set; } = true;

        /// <summary>
        ///A flag indicating whether the provider events should be enabled
        /// Note: for non EIP 1193 providers this will have no effect
        /// </summary>
        public bool EnableEvents { get; set; } = true;

        public static JsonRpcProviderOptions MetaMaskOptions => _metaMaskOptions;

        //-- 

        private static JsonRpcProviderOptions _metaMaskOptions = new JsonRpcProviderOptions()
        {
            ProviderPath = "ethereum",
            SupportsEip1193 = true
        };


    }




}
