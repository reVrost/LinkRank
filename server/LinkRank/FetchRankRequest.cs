
using System.Text.Json.Serialization;

namespace LinkRank
{
    public sealed class FetchRankRequest
    {
        [JsonPropertyName("searchWord")]
        public string SearchWord { get; set; }

        [JsonPropertyName("searchUrl")]
        public string SearchUrl { get; set; }
    }
}