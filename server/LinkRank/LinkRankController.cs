using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Caching.Memory;

namespace LinkRank
{
    [ApiController]
    public class LinkRankController : ControllerBase
    {
        private readonly ILinkRankService linkRankService;
        private readonly int CacheExpirationInMinutes = 60;
        private IMemoryCache hourlyRankerCache;

        public LinkRankController(ILinkRankService linkRankService, IMemoryCache memoryCache)
        {
            this.linkRankService = linkRankService;
            this.hourlyRankerCache = memoryCache;
        }

        [HttpPost("api/linkrank/fetch-rank")]
        public ActionResult<FetchRankResponse> FetchRank([FromBody] FetchRankRequest request)
        {
            return hourlyRankerCache.GetOrCreate(request.SearchUrl + request.SearchWord, entry =>
              {
                  Console.WriteLine(request.SearchUrl + request.SearchWord);
                  entry.SlidingExpiration = TimeSpan.FromMinutes(CacheExpirationInMinutes);
                  var rankResult = linkRankService.FetchRank(request.SearchWord, request.SearchUrl);
                  return new FetchRankResponse
                  {
                      Rankings = rankResult.Rankings,
                      SearchWord = request.SearchWord,
                      SearchUrl = request.SearchUrl,
                      Error = rankResult.Error,
                  };
              });
        }
    }
}
