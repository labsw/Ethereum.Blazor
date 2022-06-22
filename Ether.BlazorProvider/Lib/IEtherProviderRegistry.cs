using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ether.BlazorProvider
{
    public interface IEtherProviderRegistry
    {
        /// <summary>
        /// True if a single provider has been configured.
        /// </summary>
        bool HasSingleProvider { get; }

        /// <summary>
        /// Returns the only provider, this is a simplified interface for the case when only one provider has been configured.
        /// The IJsonRpcProvider is cached so GetProvider() should be performant 
        /// If more than one provider has been configuration an exception will be throw.
        /// </summary>
        ValueTask<IJsonRpcProvider> GetSingleProvider();

        /// <summary>
        /// Gets the provider by the configured name. Use AddEtherProvider() to configure a provider. 
        /// The IJsonRpcProvider is cached so GetProvider() should be performant 
        /// EtherProviderException will be throw if the provider has not been configured.
        /// </summary>
        ValueTask<IJsonRpcProvider> GetProvider(string name);
    }
}
