namespace Ether.BlazorProvider.Internal
{

    //-- RpcResponseMessage

    internal class RpcResponseMessage : IRpcResponseMessage
    {
        private readonly RpcResponseMessageDto _dto;

        public object? Id => _dto.Id;

        public string JsonRpcVersion => _dto.JsonRpcVersion;

        public IRpcResonseError? Error { get; protected set; }

        public bool HasError => _dto.HasError;

        public RpcResponseMessage(RpcResponseMessageDto dto)
        {
            _dto = dto;
            if (_dto.HasError)
                Error = new RpcResonseError(dto.Error!);
        }

        public T ResultAs<T>()
        {
            return _dto.ResultAs<T>();
        }
    }

    //-- RpcResonseError

    internal class RpcResonseError: IRpcResonseError
    {
        private RpcResonseErrorDto _dto;

        public RpcResonseError(RpcResonseErrorDto dto)
        {
            _dto = dto;
        }

        public int Code => _dto.Code;

        public string Message => _dto.Message;

        public T DataAs<T>()
        {
            return _dto.DataAs<T>();
        }
    }

}
