namespace Ether.BlazorProvider
{
    public interface IJsonRpcProvider
    {
        /// <summary>
        /// Returns a boolean indicating whether the provider extension is installed or not
        /// </summary>
        /// <returns>True if the provider extension is abailable in the browser otherwise false</returns>
        ValueTask<bool> IsAvailable();

        /// <summary>
        /// Connects to the provider requesting the account address. Exceptions can be throwed if access is denied.
        /// If the provider options SupportEip1193 is set to false then GetAccount() is called to return the account
        /// Optional timeout
        /// </summary>
        /// <returns>The account address</returns>
        ValueTask<string> Connect(TimeSpan? timeout = null);

        /// <summary>
        /// Issues a RPC request for the account address. Exceptions can be thrown on failure.
        /// </summary>
        /// <returns>The selected account address</returns>
        ValueTask<string> GetAccount();

        /// <summary>
        /// Issues a RPC request for the chain Id. Exceptions can be thrown in failure
        /// </summary>
        /// <returns>The Chain ID</returns>
        ValueTask<long> GetChainId();

        /// <summary>
        /// Execute a RPC call.
        /// Optional timeout duration
        /// </summary>
        /// <param name="request">The RPC request</param>
        /// <returns>The RPC response</returns>
        ValueTask<IRpcResponseMessage> Request(RpcRequestMessage request, TimeSpan? timeout = null);

        /// <summary>
        /// Execute a RPC call
        /// Optional timeout duration
        /// </summary>
        /// <param name="request">A JSON string that containes the RPC request</param>
        /// <returns>A JSON string with the RPC response</returns>
        ValueTask<string> Request(string request, TimeSpan? timeout = null);

        /// <summary>
        /// The options that this provider was initialised with
        /// </summary>
        JsonRpcProviderOptions Options { get; }

    }
}
