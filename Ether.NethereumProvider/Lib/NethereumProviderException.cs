namespace Ether.BlazorProvider
{
    public class NethereumProviderException : Exception
    {
        public NethereumProviderException() { }
        public NethereumProviderException(string? message) : base(message) { }
        public NethereumProviderException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
