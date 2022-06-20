using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ether.BlazorProvider.Internal
{
    [JsonObject]
    internal class RpcResponseMessageDto
    {
        [JsonProperty("id", Required = Required.Default)]
        public object? Id { get; private set; }

        [JsonProperty("jsonrpc", Required = Required.Always)]
        public string JsonRpcVersion { get; private set; }

        [JsonProperty("result", Required = Required.Default)]
        public JToken? Result { get; private set; }

        [JsonProperty("error", Required = Required.Default)]
        public RpcResonseErrorDto? Error { get; protected set; }

        [JsonIgnore]
        public bool HasError { get { return Error != null; } }

        [JsonConstructor]
        protected RpcResponseMessageDto()
        {
            JsonRpcVersion = "2.0";
        }

        protected RpcResponseMessageDto(object id)
        {
            Id = id;
            JsonRpcVersion = "2.0";
        }

        public RpcResponseMessageDto(object id, RpcResonseErrorDto error) : this(id)
        {
            Error = error;
        }

        public RpcResponseMessageDto(object id, JToken result) : this(id)
        {
            Result = result;
        }

        public T ResultAs<T>()
        {
            if (Result == null)
                throw new RpcResponseException("Error Result is null");

            T? result = Result.ToObject<T>();

            if (result == null)
                throw new RpcResponseException("Error converting response");

            return result;

        }
    }

    [JsonObject]
    internal class RpcResonseErrorDto
    {
        [JsonProperty("code")]
        public int Code { get; private set; }

        [JsonProperty("message", Required = Required.Always)]
        public string Message { get; private set; } = String.Empty;

        [JsonProperty("data")]
        public JToken? Data { get; private set; }

        public T DataAs<T>()
        {
            if (Data == null)
                throw new RpcResponseException("Error Data is null");

            T? result = Data.ToObject<T>();

            if (result == null)
                throw new RpcResponseException("Error converting data");

            return result;
        }

    }

}
