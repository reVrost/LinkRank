using System.Collections.Generic;

namespace LinkRank
{
    public sealed class RankResult
    {
        public IReadOnlyList<int> Rankings { get; set; } = new List<int>();
        public string Error { get; set; } = "";
    }
}