using System.Text.Json.Serialization;

namespace LinkRank.SearchEngine
{
    public sealed class SearchItem
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("htmlTitle")]
        public string HtmlTitle { get; set; }

        [JsonPropertyName("link")]
        public string Link { get; set; }

        [JsonPropertyName("displayLink")]
        public string DisplayLink { get; set; }
    }
}