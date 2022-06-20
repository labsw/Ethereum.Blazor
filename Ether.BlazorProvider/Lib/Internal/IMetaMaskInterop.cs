namespace Ether.BlazorProvider.Internal
{
    internal interface IMetaMaskInterop
    {
        ValueTask<bool> IsMetaMaskAvailable();

        ValueTask<string> Connect();

        ValueTask<string> GetAccount();
        ValueTask<long> GetChainId();

        ValueTask ConfigureOnChange(Action<string>? onAccountChanged, Action<long>? onChainChanged);

        ValueTask<RpcResponseMessageDto> RpcRequest(RpcRequestMessageDto request);
        ValueTask<string> RpcRequest(string request);

        ValueTask<string> SignMessage(string utf8Hex);
    }
}
