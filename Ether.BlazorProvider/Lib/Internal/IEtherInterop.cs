namespace Ether.BlazorProvider.Internal
{
    internal interface IEtherInterop
    {
        ValueTask<IJsonRpcProviderInterop> InitProvider(string name, JsonRpcProviderOptionsDto options);
    }
}
