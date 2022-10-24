using Newtonsoft.Json;
using WhiteBit.Net.Models.Enums;

namespace WhiteBit.Net.Models.Responses
{
    public class WhiteBitSocketError
    {
        [JsonProperty("message")]
        string Message { get; set; } = string.Empty;

        [JsonProperty("code")]
        SocketErrorCode? Error { get; set; } = null;

        public override string ToString() => Error is null ? string.Empty : $"{Error}: {Message}";

    }
}