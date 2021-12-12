using Microsoft.AspNetCore.WebUtilities;
using System.Text.RegularExpressions;
using vue_businesslogic.Models;

namespace vue_businesslogic.Services
{
    public class KeywordWebScraperService : IKeywordWebScraperService
    {
        private readonly string GoogleSearchUrl = "https://google.co.uk/search?num=100";

        public async Task<PostToWebScrapeResponse> GetWebScrapeResults(string url, string keywords)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException(nameof(url));
            }

            if (string.IsNullOrEmpty(keywords))
            {
                throw new ArgumentException(nameof(keywords));
            }

            string fullUrl = ConstructFullUrl(keywords);

            string html = await GetHtml(fullUrl);

            return CheckForMatches(url, html);
        }

        private PostToWebScrapeResponse CheckForMatches(string url, string html)
        {
            PostToWebScrapeResponse postToWebScrapeResponse = new();

            HashSet<int> positions = new HashSet<int>();

            // Match on the divs with the class name (kCrYT) to find the search result content
            var matches = Regex.Matches(html,
                @"<div class=""kCrYT"">[\s\S]*?</div>");

            for (int i = 0; i < matches.Count; i++)
            {
                // We only want to check the href - it's possible the href could be displayed elsewhere.
                // We only want 1 result per search result
                var secondMatch = Regex.Match(matches[i].Value, @"<a [\s\S]*?>");

                if (!secondMatch.Success)
                {
                    continue;
                }

                if (secondMatch.Value.ToString().Contains(url))
                {
                    postToWebScrapeResponse.Occurences++;

                    positions.Add(i);
                }
            }
            
            foreach (var position in positions)
            {
                if(postToWebScrapeResponse.ResultPositions == String.Empty)
                {
                    postToWebScrapeResponse.ResultPositions = position.ToString();
                }
                else
                {
                    postToWebScrapeResponse.ResultPositions = String.Concat(" ", position.ToString());
                }
            }

            return postToWebScrapeResponse;
        }

        private async Task<string> GetHtml(string fullUrl)
        {
            if (string.IsNullOrEmpty(fullUrl))
            {
                throw new ArgumentException(nameof(fullUrl));
            }

            using HttpClient httpClient = new();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Add("cookie", "CONSENT=YES+cb.20210727-07-p1.en-GB+FX+528");

            return await httpClient.GetStringAsync(fullUrl);
        }

        private string ConstructFullUrl(string keywords)
        {
            keywords = keywords.Replace(' ', '+');

            return new Uri(QueryHelpers.AddQueryString(GoogleSearchUrl, "q", keywords)).ToString();
        }
    }
}
