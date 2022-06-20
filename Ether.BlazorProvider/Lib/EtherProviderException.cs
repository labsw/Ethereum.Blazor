namespace Ether.BlazorProvider
{
    public class EtherProviderException : Exception
    {
        public EtherProviderException() { }
        public EtherProviderException(string? message) : base(message) { }
        public EtherProviderException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
