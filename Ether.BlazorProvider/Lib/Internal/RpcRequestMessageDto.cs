using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Ether.BlazorProvider.Internal
{
    [JsonObject]
    internal class RpcRequestMessageDto
    {
        [JsonProperty("id")]
        public object Id { get; private set; }

        [JsonProperty("jsonrpc", Required = Required.Always)]
        public string JsonRpcVersion { get; private set; }

        [JsonProperty("method", Required = Required.Always)]
        public string Method { get; private set; }

        [JsonProperty("params")]
        public object RawParameters { get; private set; }

        public RpcRequestMessageDto(object id, string method, params object[] parameterList)
        {
            Id = id;
            JsonRpcVersion = "2.0";
            Method = method;
            RawParameters = parameterList;
        }

        public RpcRequestMessageDto(object id, string method, Dictionary<string, object> parameterMap)
        {
            Id = id;
            JsonRpcVersion = "2.0";
            Method = method;
            RawParameters = parameterMap;
        }

        public RpcRequestMessageDto(RpcRequestMessage rpcRequestMessage)
        {
            Id = rpcRequestMessage.Id;
            JsonRpcVersion = rpcRequestMessage.RpcVersion;
            Method = rpcRequestMessage.Method;
            RawParameters = rpcRequestMessage.RawParameters;
        }
    }

    internal class RpcParametersJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.StartObject:
                    try
                    {
                        JObject jObject = JObject.Load(reader);
                        return jObject.ToObject<Dictionary<string, object>>();
                    }
                    catch (Exception)
                    {
                        throw new Exception("Request parameters can only be an associative array, list or null.");
                    }
                case JsonToken.StartArray:
                    return JArray.Load(reader).ToObject<object[]>(serializer);
                case JsonToken.Null:
                    return null;
            }
            throw new Exception("Request parameters can only be an associative array, list or null.");
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }

}
