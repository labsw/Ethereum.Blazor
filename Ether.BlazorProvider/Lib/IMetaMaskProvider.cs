namespace Ether.BlazorProvider
{
    public interface IMetaMaskProvider
    {
        /// <summary>
        /// Returns a boolean indicating whether the MetaMask extension is installed or not
        /// </summary>
        /// <returns>True if the MetaMask extension is installed otherwise false</returns>
        ValueTask<bool> IsMetaMaskAvailable();

        /// <summary>
        /// Connects to MetaMask requesting the account address. Exceptions can be throwed if access is denied.
        /// </summary>
        /// <returns>The account address</returns>
        ValueTask<string> Connect();

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
        /// Signes a message
        /// </summary>
        /// <param name="message">The text of the message</param>
        /// <returns>The signed message</returns>
        ValueTask<string> SignMessage(string message);

        /// <summary>
        /// Configures account or chain Id changes to tracked.
        /// </summary>
        ValueTask ListenForChanges();

        /// <summary>
        /// Receive account change events
        /// </summary>
        event Action<string> AccountChanged;

        /// <summary>
        /// Receive Chain ID change events.
        /// </summary>
        event Action<long> ChainIdChanged;

        /// <summary>
        /// Execute a RPC call.
        /// </summary>
        /// <param name="request">The RPC request</param>
        /// <returns>The RPC response</returns>
        ValueTask<IRpcResponseMessage> RpcRequest(RpcRequestMessage request);

        /// <summary>
        /// Execute a RPC call
        /// </summary>
        /// <param name="request">A JSON string that containes the RPC request</param>
        /// <returns>A JSON string with the RPC response</returns>
        ValueTask<string> RpcRequest(string request);
    }
}
