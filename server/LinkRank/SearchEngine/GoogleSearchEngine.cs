using System.Collections.Generic;
using System.Text.Json;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using System.Web;
using System;
using System.Net.Http;

namespace LinkRank.SearchEngine
{
    public class GoogleSearchEngine : ISearchEngine
    {
        private readonly string apiKey = Environment.GetEnvironmentVariable("key");
        private readonly string cxId = Environment.GetEnvironmentVariable("cx");
        private readonly int MaxPageSize = 10;
        private readonly int MaxResultSize = 100;
        private static readonly HttpClient client = new HttpClient();

        public Dictionary<int, string> Search(string searchText)
        {
            var googleUrl = "https://www.googleapis.com/customsearch/v1";

            var searchUrl = QueryHelpers.AddQueryString(googleUrl,
                new Dictionary<string, string>() {
                    {"key", apiKey},
                    {"cx", cxId},
                    { "q", HttpUtility.UrlEncode(searchText)},
                }
            );

            var rankedUrls = new Dictionary<int, string>();
            var tasks = new List<Task>();

            for (var startPage = 1; startPage < MaxResultSize; startPage += MaxPageSize)
            {
                var localStart = startPage;
                tasks.Add(Task.Run(() => GetPagedRanking(searchUrl, localStart, rankedUrls)));
            }

            Task.WaitAll(tasks.ToArray());
            return rankedUrls;

        }
        private async Task<Dictionary<int, string>> GetPagedRanking(string searchUrl, int startPage, Dictionary<int, string> rankedUrls)
        {
            var pagedUrl = QueryHelpers.AddQueryString(searchUrl, new Dictionary<string, string>() { { "start", startPage.ToString() }, });
            var response = await client.GetAsync(pagedUrl);
            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                throw new TooManyRequestsException();
            }

            var result = await JsonSerializer.DeserializeAsync<GoogleSearchResult>(await response.Content.ReadAsStreamAsync());

            if (result.Items != null)
            {
                for (int i = 0; i < result.Items.Count; i++)
                {
                    rankedUrls.Add(startPage + i, result.Items[i].DisplayLink);
                };
            }

            return rankedUrls;
        }
    }
}