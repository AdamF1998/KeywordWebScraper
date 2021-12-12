using Microsoft.AspNetCore.Mvc;
using vue_businesslogic.Models;
using vue_businesslogic.Services;

namespace Keyword_WebScraper_Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KeywordWebScraperController : ControllerBase
    {
        private readonly IKeywordWebScraperService _keywordWebScraperService;

        public KeywordWebScraperController(IKeywordWebScraperService keywordWebScraperService)
        {         
            _keywordWebScraperService = keywordWebScraperService;
        }

        [HttpPost(Name = "PostToWebScrape")]
        public async Task<IActionResult> Post([FromBody] PostToWebScrapeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(await _keywordWebScraperService.GetWebScrapeResults(request.Url, request.Keywords));
            }
            
        }
    }
}
