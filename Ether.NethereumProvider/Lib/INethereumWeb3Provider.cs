using Nethereum.Web3;

namespace Ether.NethereumProvider
{
    public interface INethereumWeb3Provider
    {
        /// <summary>
        /// The selected account
        /// </summary>
        string Account { get; }

        /// <summary>
        /// True if the provider is available
        /// </summary>
        ValueTask<bool> IsProviderAvailable();

        /// <summary>
        /// Connects to provider requesting account access. This must be called before any usage of IWeb3 methods
        /// </summary>
        ValueTask Connect();

        /// <summary>
        /// Returns the IWeb3 client
        /// </summary>
        /// <returns>An IWeb3 interface</returns>
        IWeb3 GetWeb3();

    }
}
