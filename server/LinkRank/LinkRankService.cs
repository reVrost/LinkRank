
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;
using System.Net;
using System.Web;

namespace LinkRank
{
    public class LinkRankService : ILinkRankService
    {
        private readonly string apiKey = "AIzaSyAuZ3As93VfnWruz9iqKbV1ZTz_LOsnLT4";
        private readonly string cxId = "9ac87101b9f6dfb4c";
        private readonly int MaxPageSize = 10;
        private readonly int MaxResultSize = 100;
        private static readonly HttpClient client = new HttpClient();
        private readonly ILogger<LinkRankService> _logger;

        private async Task<Dictionary<int, string>> GetPagedRanking(string searchUrl, int startPage, Dictionary<int, string> rankedUrls)
        {
            var pagedUrl = QueryHelpers.AddQueryString(searchUrl, new Dictionary<string, string>() { { "start", startPage.ToString() }, });
            var response = await client.GetAsync(pagedUrl);
            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                throw new TooManyRequestsException();
            }

            var result = await JsonSerializer.DeserializeAsync<GoogleSearchResult>(await response.Content.ReadAsStreamAsync());

            for (int i = 0; i < result.Items.Count; i++)
            {
                rankedUrls.Add(startPage + i, result.Items[i].DisplayLink);
            };

            Console.WriteLine(pagedUrl);
            return rankedUrls;
        }

        public RankResult FetchRank(string searchTerm, string url)
        {
            var googleUrl = "https://www.googleapis.com/customsearch/v1";

            var searchUrl = QueryHelpers.AddQueryString(googleUrl,
                new Dictionary<string, string>() {
                    {"key", apiKey},
                    {"cx", cxId},
                    { "q", HttpUtility.UrlEncode(searchTerm)},
                    { "cr", "countryAU"},
                }
            );

            var rankedUrls = new Dictionary<int, string>();
            var tasks = new List<Task>();

            for (var startPage = 1; startPage < MaxResultSize; startPage += MaxPageSize)
            {
                var localStart = startPage;
                tasks.Add(Task.Run(() => GetPagedRanking(searchUrl, localStart, rankedUrls)));
            }

            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException ex)
            {
                return (ex.InnerException is TooManyRequestsException)
                ? new RankResult { Error = "The API key used has passed its requests quota, please use another one" }
                : new RankResult { Error = "Couldn't reach search API, something went wrong, try again." };
            }

            rankedUrls.ToList().ForEach(x => Console.WriteLine(x.Key + "|" + x.Value));

            return new RankResult { Rankings = rankedUrls.Where(x => x.Value.Contains(url)).Select(x => x.Key).ToList() };
        }
    }
}
