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
        /// Javascript provider object path. 
        /// </summary>
        public string ProviderPath { get; set; } = string.Empty;

        /// <summary>
        /// Indicates that the provider supports the EIP 1193
        /// </summary>
        public bool SupportsEip1193 { get; set; } = true;

        /// <summary>
        /// Indicates that the provider supports the EIP 1102
        /// </summary>
        public bool SupportsEip1102 { get; set; } = true;

        /// <summary>
        /// Enables the processing of change events for Accounts and ChainID
        /// Note: for non EIP 1193 providers this will have no effect
        /// </summary>
        public bool EnableEvents { get; set; } = true;

        /// <summary>
        /// Default MetaMask provider options. Events are enabled by default.
        /// </summary>
        public static JsonRpcProviderOptions MetaMaskOptions => _metaMaskOptions;


        //-- 

        private static JsonRpcProviderOptions _metaMaskOptions = new JsonRpcProviderOptions()
        {
            ProviderPath = "ethereum",
            SupportsEip1193 = true,
            SupportsEip1102 = true,
            EnableEvents = true
        };

    }




}
