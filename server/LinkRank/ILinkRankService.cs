namespace LinkRank
{
    public interface ILinkRankService
    {
        RankResult FetchRank(string searchTerm, string url);
    }
}