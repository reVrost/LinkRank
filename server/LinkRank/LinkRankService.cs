
using System.Linq;
using System;
using LinkRank.SearchEngine;
using System.Collections.Generic;

namespace LinkRank
{
    public class LinkRankService : ILinkRankService
    {
        private readonly ISearchEngine googleSearchEngine;
        public LinkRankService(ISearchEngine googleSearchEngine)
        {
            this.googleSearchEngine = googleSearchEngine;
        }

        public RankResult FetchRank(string searchTerm, string url)
        {
            var googleRankedUrls = new Dictionary<int, string>();
            var bingRankedUrls = new Dictionary<int, string>();
            try
            {
                googleRankedUrls = googleSearchEngine.Search(searchTerm);
                return new RankResult { Rankings = googleRankedUrls.Where(x => x.Value.Contains(url)).Select(x => x.Key).ToList() };
            }
            catch (AggregateException ex)
            {
                return (ex.InnerException is TooManyRequestsException)
                ? new RankResult { Error = "The API key used has passed its requests quota, please use another one" }
                : new RankResult { Error = "Couldn't reach search API, something went wrong, try again." };
            }
        }
    }
}
