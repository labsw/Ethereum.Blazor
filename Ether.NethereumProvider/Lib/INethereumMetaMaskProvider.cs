using Nethereum.Web3;

namespace Ether.NethereumProvider
{
    public interface INethereumMetaMaskProvider
    {
        /// <summary>
        /// The selected account
        /// </summary>
        string Account { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>True if MetaMask is available otherwise false</returns>
        ValueTask<bool> IsMetaMaskAvailable();

        /// <summary>
        /// Connects to MetaMask requesting account access. This must be called before any usage of IWeb3 methods
        /// </summary>
        ValueTask Connect();

        /// <summary>
        /// Creates a IWeb3 instance
        /// </summary>
        /// <returns>An IWeb3 interface</returns>
        ValueTask<IWeb3> CreateWeb3();

    }
}
