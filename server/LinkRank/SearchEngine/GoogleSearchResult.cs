using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace LinkRank.SearchEngine
{
    public sealed class GoogleSearchResult
    {
        [JsonPropertyName("items")]
        public List<SearchItem> Items { get; set; }
    }
}