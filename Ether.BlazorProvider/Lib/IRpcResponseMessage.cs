using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ether.BlazorProvider
{
    /// <summary>
    /// The RPC response message returned after a Request() call
    /// </summary>
    public interface IRpcResponseMessage
    {
        /// <summary>
        /// The Id as supplied by the request message
        /// </summary>
        object? Id { get; }

        /// <summary>
        /// The JSON RPC version
        /// </summary>
        string JsonRpcVersion { get; }

        /// <summary>
        /// The result of the Request() call if successful. Its up to the client to cast result to the correct data structure.
        /// </summary>
        T ResultAs<T>();

        /// <summary>
        /// Is true if the Request() call returned an RPC error, See Error property for details
        /// </summary>
        bool HasError { get; }

        /// <summary>
        /// A RPC errror that was returned from a Request() call, null if there is no error
        /// </summary>
        IRpcResponseError? Error { get; }

    }

    /// <summary>
    /// A RPC response error
    /// </summary>
    public interface IRpcResponseError
    {
        /// <summary>
        /// The RPC error code, see the Ethereum RPC spec for details
        /// </summary>
        int Code { get; }

        /// <summary>
        /// The RPC error message, this is optional so could be empty
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Additional error data, see the Ethereum RPC spec for details
        /// </summary>
        T DataAs<T>();
    }

}
