using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ether.BlazorProvider
{
    public interface IRpcResponseMessage
    {
        object? Id { get; }
        string JsonRpcVersion { get; }
        T ResultAs<T>();

        IRpcResonseError? Error { get; }
        bool HasError { get; }
    }

    public interface IRpcResonseError
    {
        int Code { get; }
        string Message { get; }
        T DataAs<T>();
    }

}
