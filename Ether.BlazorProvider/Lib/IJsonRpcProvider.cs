namespace Ether.BlazorProvider
{
    public interface IJsonRpcProvider
    {
        /// <summary>
        /// Receive account change events
        /// </summary>
        event Action<string> AccountChanged;

        /// <summary>
        /// Receive Chain ID change events.
        /// </summary>
        event Action<long> ChainIdChanged;

        /// <summary>
        /// The options that the provider was configured with.
        /// </summary>
        JsonRpcProviderOptions Options { get; }

        /// <summary>
        /// Name of the provider as configured in the AddEtherProvider()
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns a boolean indicating whether the provider extension is installed or not
        /// </summary>
        /// <returns>True if the provider extension is abailable in the browser otherwise false</returns>
        ValueTask<bool> IsAvailable();

        /// <summary>
        /// If the provider was configured with SupportEip1102 = true then access to the wallet is requested. Exceptions can be throwed if access is denied.
        /// If the provider was configured with SupportEip1102 = false then GetAccount() is called to return the current wallets's account.
        /// An optional timeout can be supplied with an exception being throw if the timeout expires
        /// </summary>
        /// <returns>The account address</returns>
        ValueTask<string> Connect(TimeSpan? timeout = null);

        /// <summary>
        /// Issues a RPC request for the account address. Exceptions can be thrown on failure.
        /// </summary>
        /// <returns>The current account address</returns>
        ValueTask<string> GetAccount();

        /// <summary>
        /// Issues a RPC request for the chain Id. Exceptions can be thrown in failure
        /// </summary>
        /// <returns>The current Chain ID</returns>
        ValueTask<long> GetChainId();

        /// <summary>
        /// Execute a RPC call.
        /// Optional timeout duration
        /// </summary>
        /// <param name="request">The RPC request</param>
        /// <returns>The RPC response</returns>
        ValueTask<IRpcResponseMessage> Request(RpcRequestMessage request, TimeSpan? timeout = null);

        /// <summary>
        /// Execute a RPC call. The request JSON should conform to the Ethererum RPC specification.
        /// Optional timeout duration
        /// </summary>
        /// <param name="request">A JSON string that containes the RPC request</param>
        /// <returns>A JSON string with the RPC response</returns>
        ValueTask<string> Request(string request, TimeSpan? timeout = null);

        /// <summary>
        /// Returns an interface that can sign messages
        /// </summary>
        ISigner GetSigner();

    }
}
