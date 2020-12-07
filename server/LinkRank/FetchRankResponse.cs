
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LinkRank
{
    public sealed class FetchRankResponse
    {
        [JsonPropertyName("searchWord")]
        public string SearchWord { get; set; }

        [JsonPropertyName("searchUrl")]
        public string SearchUrl { get; set; }

        [JsonPropertyName("rankings")]
        public IReadOnlyList<int> Rankings { get; set; }

        [JsonPropertyName("error")]
        public string Error { get; set; }
    }
}