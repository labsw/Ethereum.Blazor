using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ether.BlazorProvider
{
    public class RpcResponseException : Exception
    {
        public RpcResponseException() { }
        public RpcResponseException(string? message) : base(message) { }
        public RpcResponseException(string? message, Exception? innerException) : base(message, innerException) { }
    }

    public class RpcResponseErrorException : RpcResponseException
    {
        public RpcResponseErrorException(IRpcResonseError rpcError) : base(rpcError.Message)
        {
            RpcError = rpcError;
        }

        public IRpcResonseError RpcError { get; }
    }
}
