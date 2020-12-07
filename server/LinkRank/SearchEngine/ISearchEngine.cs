using System.Collections.Generic;
namespace LinkRank.SearchEngine
{
    public interface ISearchEngine
    {
        /// <summary>
        /// Given a search text returns a dictionary with all the urls returned from the search keyed by its ranking
        /// </summary>
        Dictionary<int, string> Search(string searchText);
    }
}