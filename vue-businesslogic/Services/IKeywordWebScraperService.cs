using vue_businesslogic.Models;

namespace vue_businesslogic.Services
{
    public interface IKeywordWebScraperService
    {
        Task<PostToWebScrapeResponse> GetWebScrapeResults(string url, string keywords);
    }
}
